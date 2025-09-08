using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Grabber : MonoBehaviour
{
    SphereCollider reachCollider;
    private List<IGrabbable> grabCandidates = new List<IGrabbable>();
    private IGrabbable currentGrabbed;


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





    }

    //private void Update()
    //{
    //    if (currentGrabbed != null)
    //}

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
