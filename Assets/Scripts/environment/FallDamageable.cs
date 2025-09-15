using UnityEngine;

[RequireComponent(typeof(Damageable))]
public class FallDamageable : MonoBehaviour
{
    Damageable damageable;
    float lastHeight;

    private void Awake()
    {
        damageable = GetComponent<Damageable>();
        lastHeight = transform.position.y;
    }

    private void OnCollisionEnter(Collision collision)
    {
        float currentHeight = transform.position.y;
        if (currentHeight < lastHeight) { damageable.Damage(lastHeight - currentHeight, collision.contacts[0].point, collision.contacts[0].normal); }
        lastHeight = currentHeight;
    }
}
