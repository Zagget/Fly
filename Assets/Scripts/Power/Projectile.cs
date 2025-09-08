using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    Rigidbody rb;
    public float damage;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.TryGetComponent<Damageable>(out Damageable damageable))
        {
            damageable.Damage(damage, collision.contacts[0].point, rb.linearVelocity * rb.mass);
        }
        Destroy(gameObject);
    }
}
