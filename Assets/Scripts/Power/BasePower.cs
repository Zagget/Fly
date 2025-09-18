using System;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class BasePower : ScriptableObject
{
    public float Cooldown { get; protected set; }

    protected RigManager rigManager;
    protected float currentCharge;
    [SerializeField] protected float duration = 0;
    
    [SerializeField] protected int maximumCharge = 10;
    public int MaximumCharge { get {  return maximumCharge; } }


    PowerManager powerManager;
    float timeStarted;

    public virtual void Activate(RigManager rigManager, float currentCharge, PowerManager powerManager)
    {
        this.rigManager = rigManager;
        if (this.rigManager == null) return;
        this.currentCharge = currentCharge;
        this.powerManager = powerManager;
        powerManager.continues += Continues;
        timeStarted = Time.time;

        Start();
    }

    public void Continues()
    {
        if (Time.time - timeStarted >= duration)
        {
            End();
            powerManager.continues -= Continues;
        }
        else
        {
            Update();
        }
    }

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