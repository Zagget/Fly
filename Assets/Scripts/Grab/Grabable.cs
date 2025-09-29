using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Grabable : MonoBehaviour
{
    [HideInInspector]
    public Rigidbody rb;
    [HideInInspector]
    public Collider grabCollider;

    public SphereCollider triggerCollider;
    [HideInInspector]
    public HashSet<Collider> blockers = new HashSet<Collider>();

    readonly List<(Transform t, int layer)> savedLayer = new();

    public int weight;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        grabCollider = GetComponent<Collider>();
        if (triggerCollider == null)
        {
            Debug.Log("Asign trigger collider to " + gameObject);
        }
        
    }


    public void OnGrab(Transform hand)
    {
        rb.isKinematic = true;
        savedLayer.Clear();
        SetAndSaveLayer(transform, 0);
        // Grabbing at objects current pos, rot.
        //Vector3 offset = hand.InverseTransformPoint(transform.position);
        //Quaternion rotationOffset = Quaternion.Inverse(hand.rotation) * transform.rotation;
        //transform.SetParent(hand);

        //Vector3 offset = transform.position;
        //Quaternion rotationOffset = transform.rotation;
        //transform.localPosition = offset;
        //transform.localRotation = rotationOffset;
    }

    public void OnRelease(Vector3 velocity, Vector3 rotationSpeed)
    {
        rb.isKinematic = false;
        RestoreLayer();
        transform.SetParent(null, true);
        rb.linearVelocity = velocity;

        rb.angularVelocity = Vector3.ClampMagnitude(rotationSpeed, 10f);
    }

    private void SetAndSaveLayer(Transform t, int layer)
    {
        if (t == null) return;
        savedLayer.Add((t, t.gameObject.layer));
        t.gameObject.layer = layer;
        foreach (Transform child in t)
        {
            SetAndSaveLayer(child, layer);
        }
    }
    private void RestoreLayer()
    {
        foreach (var (t, layer) in savedLayer)
        {
            if (t == null) continue;
            t.gameObject.layer = layer;
        }
    }


}
