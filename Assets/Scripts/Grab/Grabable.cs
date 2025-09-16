using UnityEngine;

public class Grabable : MonoBehaviour, IGrabbable
{
    public Rigidbody rb => GetComponent<Rigidbody>();
    [SerializeField] int weight;
    public int Weight => weight;
    public void OnGrab(Transform hand)
    {
        rb.isKinematic = true;

        // Grabbing at objects current pos, rot.
        Vector3 offset = hand.InverseTransformPoint(transform.position);
        Quaternion rotationOffset = Quaternion.Inverse(hand.rotation) * transform.rotation;
        transform.SetParent(hand);
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
}
