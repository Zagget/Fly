using System;
using UnityEngine;

[Serializable]
public abstract class BasePower : ScriptableObject
{
    public float Cooldown { get; protected set; }
    protected float currentCharge;

    [SerializeField] protected int maximumCharge = 10;
    public int MaximumCharge { get { return maximumCharge; } }


    protected RigManager rigManager;
    protected PowerManager powerManager;
    protected float timeStarted;

    public virtual void Activate(RigManager rigManager, float currentCharge, PowerManager powerManager)
    {
        this.rigManager = rigManager;
        if (this.rigManager == null) return;
        this.currentCharge = currentCharge;
        this.powerManager = powerManager;
        timeStarted = Time.time;
    }

    public virtual void DeactivateToggle() { }

    public abstract void Continues();

    /// <summary>
    /// Gets called once when activating
    /// </summary>
    public virtual void Start() { }

    /// <summary>
    ///  Gets called once every frame as long as the duration has not run out
    /// </summary>
    public virtual void Update() { }

    /// <summary>
    /// Gets called one time instead of update if the duration has run out
    /// </summary>
    public virtual void End() { }
}