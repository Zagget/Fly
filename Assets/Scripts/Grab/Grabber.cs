using Oculus.Interaction;
using Oculus.Interaction.Input;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Grabber : MonoBehaviour
{
    [SerializeField] LayerMask solidMask;

    SphereCollider reachCollider;

    private List<Grabable> grabCandidates = new List<Grabable>();
    [HideInInspector]
    public Grabable currentGrabbed;

    private Vector3 currentLinearVelocity;
    private Vector3 currentAngularVelocity;

    private Vector3 prevPos;
    private Quaternion prevRot;

    private Vector3 desiredPos;
    private Quaternion desiredRot;

    private Vector3 lastSafePos;
    private Vector3 delta;
    private Vector3 dir;
    private Vector3 entryNormal;
    

    public static Action<Grabable> onGrab;
    public static Action<Grabable> onRelease;

    static readonly Collider[] blockColliders = new Collider[32];

    void Start()
    {
        reachCollider = GetComponent<SphereCollider>();

        prevPos = transform.position;
        prevRot = transform.rotation;
    }

    public void OnGrabInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Grab();
        }
        else if (context.canceled)
        {
            Release();
        }
    }

    void Grab()
    {
        if (grabCandidates.Count == 0) return;
        Grabable closestGrab = null;
        var origin = transform.position;

        foreach (var candidate in grabCandidates)
        {
            if (closestGrab == null)
            {
                closestGrab = candidate;
                continue;
            }
            if ((candidate.rb.position - origin).sqrMagnitude <
                (closestGrab.rb.position - origin).sqrMagnitude)
            {
                closestGrab = candidate;
            }
        }
        if (PowerProgression.Instance.powerLevel < closestGrab.weight) return;

        currentGrabbed = closestGrab;
        closestGrab.OnGrab(transform);
        IgnoreCollisions(currentGrabbed.grabCollider, RigManager.instance.currentCollider);
        onGrab?.Invoke(currentGrabbed);
    }

    void Release()
    {
        if (currentGrabbed == null) return;

        Debug.Log("blä Released");
        currentGrabbed.OnRelease(currentLinearVelocity, currentAngularVelocity);
        StartCoroutine(RestoreCollisionsLater(currentGrabbed.grabCollider, RigManager.instance.currentCollider));
        currentGrabbed = null;
        onRelease?.Invoke(currentGrabbed);
    }

    private void Update()
    {
        currentLinearVelocity = (transform.position - prevPos) / Time.deltaTime;
        // Angular difference calculation
        Quaternion deltaRot = transform.rotation * Quaternion.Inverse(prevRot);
        //convert to degrees
        deltaRot.ToAngleAxis(out float angleInDegrees, out Vector3 rotationAxis);
        // Convert to radians and divide by delta time to get angular velocity
        currentAngularVelocity = rotationAxis * angleInDegrees * Mathf.Deg2Rad / Time.deltaTime;

        prevPos = transform.position;
        prevRot = transform.rotation;

        desiredPos = transform.position;
        desiredRot = transform.rotation;


        MoveGrabbedItem();

    }



    private void MoveGrabbedItem()
    {
        if (currentGrabbed != null)
        {
            

            Transform probe = currentGrabbed.triggerCollider.transform;

            for (int i = 0; i < 3; i++)
            {
                probe.SetPositionAndRotation(desiredPos, desiredRot);
                int n = 0;  
                entryNormal = Vector3.zero;
                if (currentGrabbed.triggerCollider is SphereCollider sphere)
                {
                    n = Physics.OverlapSphereNonAlloc(probe.position, sphere.radius, blockColliders,
                                                        solidMask, QueryTriggerInteraction.Ignore);
                }
                if (currentGrabbed.triggerCollider is CapsuleCollider capsule)
                {
                    GetCapsulePoints(capsule, probe.position, out Vector3 point0, out Vector3 point1);
                    n = Physics.OverlapCapsuleNonAlloc(point0, point1, capsule.radius, blockColliders,
                                                        solidMask, QueryTriggerInteraction.Ignore);
                    if (n == 0)
                    {
                        lastSafePos = probe.position;
                        
                    }
                    delta = transform.position - lastSafePos;
                    dir = delta.normalized;
                    GetCapsulePoints(capsule, lastSafePos, out point0, out point1);
                    if (Physics.CapsuleCast(point0, point1, capsule.radius, dir, out RaycastHit hit, delta.magnitude, solidMask, QueryTriggerInteraction.Ignore))
                    {
                        entryNormal = hit.normal;
                    }
                    
                }

                //int n = Physics.OverlapSphereNonAlloc(probe.position, currentGrabbed.triggerCollider.radius, blockColliders,
                //solidMask, QueryTriggerInteraction.Ignore);



                
                bool moved = false;
                for (int j = 0; j < n; j++)
                {
                    var collider = blockColliders[j];
                    
                    if (Physics.ComputePenetration(
                        currentGrabbed.triggerCollider, desiredPos, desiredRot,
                        collider, collider.transform.position, collider.transform.rotation,
                        out Vector3 direction, out float distance))
                    {
                        bool suspicious = Vector3.Dot(direction, entryNormal) < 0;

                        if (suspicious && entryNormal != Vector3.zero)
                        {
                            //Debug.Log("Suspicious direction change");
                            direction = entryNormal;
                            distance = Vector3.Dot(transform.position - lastSafePos, entryNormal);
                        }



                        //Debug.Log(distance);
                        desiredPos += direction * distance;
                        Debug.Log("desiredPos: " + desiredPos + "\nsuspicious: " + suspicious + ", n: " + n + ", entryNormal: " + entryNormal + ", lastSafePos: " + lastSafePos);
                        moved = true;
                    }
                }
                if (!moved) break;
            }

            currentGrabbed.transform.position = desiredPos;
            currentGrabbed.transform.rotation = desiredRot;
            //Debug.Log(lastSafePos);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Grabbable")) return;

        Grabable grabable = other.GetComponent<Grabable>();

        if (grabable != null && !grabCandidates.Contains(grabable))
        {
            grabCandidates.Add(grabable);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Grabbable")) return;

        Grabable grabable = other.GetComponent<Grabable>();
        if (grabable != null && grabCandidates.Contains(grabable))
        {
            grabCandidates.Remove(grabable);
        }
    }

    void IgnoreCollisions(Collider collider1, Collider collider2)
    {
        Physics.IgnoreCollision(collider1, collider2, true);
    }

    IEnumerator RestoreCollisionsLater(Collider collider1, Collider collider2)
    {
        yield return new WaitForSeconds(0.3f);
        Physics.IgnoreCollision(collider1, collider2, false);
    }

    void GetCapsulePoints(CapsuleCollider capsule, Vector3 atPosition, out Vector3 point0, out Vector3 point1)
    {
        CapsuleCollider unrealCapsule = capsule;
        unrealCapsule.transform.position = atPosition;
        Vector3 center = unrealCapsule.transform.TransformPoint(unrealCapsule.center);
        Vector3 axis = unrealCapsule.direction == 0 ? Vector3.right : (unrealCapsule.direction == 1 ? Vector3.up : Vector3.forward);
        
        float halfHeight = Mathf.Max(0, (unrealCapsule.height / 2) - unrealCapsule.radius);
        point0 = center + axis * halfHeight;
        point1 = center - axis * halfHeight;
    }


}