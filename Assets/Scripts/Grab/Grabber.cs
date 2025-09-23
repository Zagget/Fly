using Oculus.Interaction.Input;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Grabber : MonoBehaviour
{
    GameObject player;

    SphereCollider reachCollider;
    private List<IGrabbable> grabCandidates = new List<IGrabbable>();
    private IGrabbable currentGrabbed;
    private Vector3 currentLinearVelocity;
    private Vector3 currentAngularVelocity;
    private Vector3 prevPos;
    private Quaternion prevRot;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
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
        IGrabbable closestGrab = null;
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
        if (PowerProgression.Instance.powerLevel < closestGrab.Weight) return;
        currentGrabbed = closestGrab;
        closestGrab.OnGrab(transform);
        IgnoreCollisions(currentGrabbed.grabcollider, player.GetComponent<Collider>());
    }

    void Release()
    {
        if (currentGrabbed == null) return;

        Debug.Log("blä Released");
        currentGrabbed.OnRelease(currentLinearVelocity, currentAngularVelocity);
        StartCoroutine(RestoreCollisionsLater(currentGrabbed.grabcollider, player.GetComponent<Collider>()));
        currentGrabbed = null;
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
    }

    private void FixedUpdate()
    {
        if (currentGrabbed != null)
        {
            Vector3 targetPos = transform.position;
            Quaternion targetRot = transform.rotation;
            Collider collider = currentGrabbed.grabcollider;

            Vector3 currentPos = currentGrabbed.rb.position;
            Quaternion currentRot = currentGrabbed.rb.rotation;

            Vector3 delta = targetPos - currentPos;
            float distance = delta.magnitude;
            if (distance > 0.0001f)
            {
                Vector3 direction = delta.normalized;
                if (collider is CapsuleCollider)
                {
                    GetCapsuleShape(collider as CapsuleCollider, out Vector3 pointA, out Vector3 pointB, out float radius);

                    if (Physics.CapsuleCast(pointA, pointB, radius, direction, out RaycastHit hit, distance, LayerMask.GetMask("Default", "Grabbable"), QueryTriggerInteraction.Ignore))
                    {
                        targetPos = currentPos + direction * Mathf.Max(0, hit.distance - 0.001f);
                    }
                }
            }
            currentGrabbed.rb.MovePosition(targetPos);

            currentGrabbed.rb.MoveRotation(targetRot);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Grabbable")) return;

        IGrabbable grabbable = other.GetComponent<IGrabbable>();

        if (grabbable != null && !grabCandidates.Contains(grabbable))
        {
            grabCandidates.Add(grabbable);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Grabbable")) return;

        IGrabbable grabbable = other.GetComponent<IGrabbable>();
        if (grabbable != null && grabCandidates.Contains(grabbable))
        {
            grabCandidates.Remove(grabbable);
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

    void GetCapsuleShape(CapsuleCollider collider, out Vector3 pointA, out Vector3 pointB, out float radius)
    {
        radius = collider.radius;
        Vector3 center = collider.transform.TransformPoint(collider.center);
        Vector3 axis = (collider.direction == 0) ? collider.transform.right :
                   (collider.direction == 1) ? collider.transform.up :
                                         collider.transform.forward;

        float half = Mathf.Max(0, 0.5f * collider.height - radius);
        pointA = center + axis * half;
        pointB = center - axis * half;
    }
}