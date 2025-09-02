using UnityEngine;

public class FlyingTowrdsView : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] float speed;
    [SerializeField] float deceleration;

    Vector3 forward;
    Vector3 velocity;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        forward = transform.forward;
    }

    private void FixedUpdate()
    {
        velocity += forward * speed;
        velocity *= deceleration;

        rb.linearVelocity = velocity;
    }
}
