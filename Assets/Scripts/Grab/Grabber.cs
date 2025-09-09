using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Grabber : MonoBehaviour
{
    SphereCollider reachCollider;
    private List<IGrabbable> grabCandidates = new List<IGrabbable>();
    private IGrabbable currentGrabbed;
    private Vector3 currentLinearVelocity;
    private Vector3 currentAngularVelocity;
    private Vector3 prevPos;
    private Quaternion prevRot;


    void Start()
    {
        reachCollider = GetComponent<SphereCollider>();
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
        currentGrabbed = closestGrab;
        closestGrab.OnGrab(transform);
    }

    void Release()
    { 
        if (currentGrabbed == null) return;
        currentGrabbed.OnRelease(currentLinearVelocity, currentAngularVelocity);
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

    private void OnTriggerEnter(Collider other)
    {
        IGrabbable grabbable = other.GetComponent<IGrabbable>();
        if (grabbable != null && !grabCandidates.Contains(grabbable))
        {
            grabCandidates.Add(grabbable);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        IGrabbable grabbable = other.GetComponent<IGrabbable>();
        if (grabbable != null && grabCandidates.Contains(grabbable))
        {
            grabCandidates.Remove(grabbable);
        }
    }

}
