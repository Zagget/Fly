using Unity.Mathematics;
using UnityEngine;

public class Grabable : MonoBehaviour, IGrabbable
{
    public Rigidbody rb => GetComponent<Rigidbody>();
    public int weight => 0;
    public void OnGrab(Transform hand)
    {
        rb.isKinematic = true;
        rb.detectCollisions = false;

        Vector3 offset = hand.InverseTransformPoint(transform.position);
        Quaternion rotationOffset = Quaternion.Inverse(hand.rotation) * transform.rotation;
        transform.SetParent(hand);
        transform.localPosition = offset;
        transform.localRotation = rotationOffset;
    }

    public void OnRelease(Vector3 velocity, Vector3 rotationSpeed)
    {
        transform.SetParent(null, true);
        rb.isKinematic = false;
        rb.detectCollisions = true;
        rb.linearVelocity = velocity;

        rb.angularVelocity = Vector3.ClampMagnitude(rotationSpeed, 10f);
    }
}
