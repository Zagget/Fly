using UnityEngine;

public class FallDamageable : MonoBehaviour
{
    [SerializeField] float forceMultiplier = 0.5f;

    Damageable[] damageables;

    private void Awake()
    {
        damageables = GetComponentsInChildren<Damageable>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Vector3 modifiedVelocity = collision.relativeVelocity * forceMultiplier;
        float force = modifiedVelocity.magnitude;
        foreach (Damageable damageable in damageables)
        {
            damageable.Damage(force, collision.GetContact(0).point, modifiedVelocity);
        }

        Debug.Log(force);
    }
}
