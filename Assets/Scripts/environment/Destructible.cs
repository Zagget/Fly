using UnityEngine;

public class Destructible : Damageable
{
    [SerializeField] GameObject ShatteredVersionPrefab;

    public override void Damage(float damage, Vector3 point, Vector3 force)
    {
        health -= damage;
        if (health <= 0) Destroy(point, force);
    }

    private void Destroy(Vector3 point, Vector3 force)
    {
        if (ShatteredVersionPrefab != null)
        {
            GameObject shatteredVersion = Instantiate(ShatteredVersionPrefab, transform.position, transform.rotation);
            shatteredVersion.transform.localScale = transform.localScale;
            if (shatteredVersion.TryGetComponent<ShatteredObject>(out ShatteredObject shatteredObject)) shatteredObject.Explode(point, force);
            else Destroy(shatteredVersion);
        }
        Destroy(gameObject);
    }
}
