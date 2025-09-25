using UnityEngine;

public class Grabable : MonoBehaviour
{
    public Rigidbody rb { get; }

    public Collider grabCollider;
    public SphereCollider triggerCollider;

    public int weight;
    


    public void OnGrab(Transform hand)
    {
        rb.isKinematic = true;

        // Grabbing at objects current pos, rot.
        Vector3 offset = hand.InverseTransformPoint(transform.position);
        Quaternion rotationOffset = Quaternion.Inverse(hand.rotation) * transform.rotation;
        transform.SetParent(hand);

        //Vector3 offset = transform.position;
        //Quaternion rotationOffset = transform.rotation;
        transform.localPosition = offset;
        transform.localRotation = rotationOffset;
    }

    public void OnRelease(Vector3 velocity, Vector3 rotationSpeed)
    {
        rb.isKinematic = false;
        transform.SetParent(null, true);
        rb.linearVelocity = velocity;

        rb.angularVelocity = Vector3.ClampMagnitude(rotationSpeed, 10f);
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }

    private void OnTriggerExit(Collider other)
    {

    }
}
