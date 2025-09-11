using UnityEngine;

public abstract class Damageable : MonoBehaviour
{
    [SerializeField] protected float health;
    public abstract void Damage(float damage, Vector3 point, Vector3 force);
}
