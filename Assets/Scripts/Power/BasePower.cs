using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Base for all powers
/// </summary>
public class BasePower
{
    public float speedFactor;
    public float damage;

    public float cooldown;

    protected int maximumCharge = 10;

    public BasePower(float speedFactor = 0f, float damage = 0f, float cooldown = 1f)
    {
        this.speedFactor = speedFactor;
        this.damage = damage;
        this.cooldown = cooldown;
    }


    /// <summary>
    /// Activates whatever power
    /// </summary>
    public virtual void Activate(Rigidbody player, float currentCharge)
    {

    }

    /// <summary>
    ///  
    /// </summary>
    public virtual void Damage(Rigidbody obj, float currentCharge)
    {

    }
}