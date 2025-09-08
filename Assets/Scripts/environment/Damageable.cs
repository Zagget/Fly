using UnityEngine;

public abstract class Damageable : MonoBehaviour
{
    [SerializeField] protected float health;
    public abstract void Damage(float damage);
}
