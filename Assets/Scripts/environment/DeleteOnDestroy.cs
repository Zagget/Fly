using UnityEngine;

public class DeleteOnDestroy : Damageable
{
    public override void Damage(float damage)
    {
        health -= damage;
        if (health < 0) Destroy(gameObject);
    }
}
