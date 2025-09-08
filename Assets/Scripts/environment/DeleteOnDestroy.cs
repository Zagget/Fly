using UnityEngine;

public class DeleteOnDestroy : Damageable
{
    public override void Damage(float damage, Vector3 point, Vector3 force)
    {
        health -= damage;
        if (health < 0) Destroy(gameObject);
    }
}
