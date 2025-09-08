using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    public float damage;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.TryGetComponent<Damageable>(out Damageable damageable)) damageable.Damage(damage);
        Destroy(gameObject);
    }
}
