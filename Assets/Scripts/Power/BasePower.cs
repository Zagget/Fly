using Unity.VisualScripting;
using UnityEngine;

public class BasePower
{
    public float baseDashSpeed;
    public float damage;

    public float cooldown;

    protected int maximumCharge = 10;

    public BasePower(float baseDashSpeed, float damage, float cooldown)
    {
        this.baseDashSpeed = baseDashSpeed;
        this.damage = damage;
        this.cooldown = cooldown;
    }

    public virtual void Activate(Rigidbody player, float currentCharge)
    {

    }

    public virtual void Damage()
    {

    }
}