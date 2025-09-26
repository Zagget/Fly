using Oculus.Interaction;
using Oculus.Interaction.Input;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Grabber : MonoBehaviour
{

    SphereCollider reachCollider;

    private List<Grabable> grabCandidates = new List<Grabable>();
    public Grabable currentGrabbed;

    private Vector3 currentLinearVelocity;
    private Vector3 currentAngularVelocity;

    private Vector3 prevPos;
    private Quaternion prevRot;

    private Vector3 desiredPos;
    private Quaternion desiredRot;

    public static Action<Grabable> onGrab;
    public static Action<Grabable> onRelease;

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


        //Move 
        MoveGrabbedItem();

    }

    private void MoveGrabbedItem()
    {
        desiredPos = transform.position;
        desiredRot = transform.rotation;
        if (currentGrabbed != null)
        {
            Constraint();
            currentGrabbed.transform.position = desiredPos;

            currentGrabbed.transform.rotation = desiredRot;
        }
    }

    private void Constraint()
    {
        for (int i = 0; i < 3; i++)
        {
            bool moved = false;
            foreach (var c in currentGrabbed.blockers)
            {
                if (c == null) continue;


                if (Physics.ComputePenetration(
                    currentGrabbed.triggerCollider, desiredPos, desiredRot,
                    c, c.transform.position, c.transform.rotation,
                    out Vector3 direction, out float distance))
                {
                    desiredPos += direction * distance;
                    moved = true;
                }
            }
            if (!moved) break;
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

   
}