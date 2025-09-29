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

    public static Action<Grabable> onGrab;
    public static Action<Grabable> onRelease;

    static readonly Collider[] buf = new Collider[32];

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

    private void FixedUpdate()
    {
        
        
    }

    private void MoveGrabbedItem()
    {
        if (currentGrabbed != null)
        {
            desiredPos = transform.position;
            desiredRot = transform.rotation;

            Transform probe = currentGrabbed.triggerCollider.transform;

            for (int i = 0; i < 3; i++)
            {
                probe.SetPositionAndRotation(desiredPos, desiredRot);

                int n = Physics.OverlapSphereNonAlloc(probe.position, currentGrabbed.triggerCollider.radius, buf,
                                                        solidMask, QueryTriggerInteraction.Ignore);
                Debug.Log(n);
                bool moved = false;
                for (int j = 0; j < n; j++)
                {
                    var collider = buf[j];
                    Debug.Log(collider.transform.parent);
                    if (Physics.ComputePenetration(
                        currentGrabbed.triggerCollider, desiredPos, desiredRot,
                        collider, collider.transform.position, collider.transform.rotation,
                        out Vector3 direction, out float distance))
                    {
                        desiredPos += direction * (distance + 0.001f);
                        moved = true;
                    }
                }
                if (!moved) break;
            }

            currentGrabbed.transform.position = desiredPos;
            currentGrabbed.transform.rotation = desiredRot;

        }
    }

    private void Constraint()
    {
        

        
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

   
}