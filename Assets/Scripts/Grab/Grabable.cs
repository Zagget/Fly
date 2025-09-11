using UnityEngine;

public class Grabable : MonoBehaviour, IGrabbable
{
    public Rigidbody rb => GetComponent<Rigidbody>();
    [SerializeField] int weight;
    public int Weight => weight;
    public void OnGrab(Transform hand)
    {
        rb.isKinematic = true;
        rb.detectCollisions = false;
        transform.SetParent(hand, false);
    }

    public void OnRelease(Vector3 velocity, Vector3 rotationSpeed)
    {
        transform.SetParent(null, true);
        rb.isKinematic = false;
        rb.detectCollisions = true;
        rb.linearVelocity = velocity;
        rb.angularVelocity = rotationSpeed;
    }
}
