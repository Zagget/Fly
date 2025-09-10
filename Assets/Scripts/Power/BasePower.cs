using System;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class BasePower : ScriptableObject
{
    public float Cooldown { get; protected set; }

    protected Rigidbody playersRigidbody;
    protected float currentCharge;
    
    [SerializeField] protected int maximumCharge = 10;
    public int MaximumCharge { get {  return maximumCharge; } }
    [SerializeField] protected float duration = 0;

    PowerManager powerManager;

    float timeStarted;

    public virtual void Activate(Rigidbody player, float currentCharge, PowerManager powerManager)
    {
        playersRigidbody = player;
        if (playersRigidbody == null) return;
        this.currentCharge = currentCharge;
        this.powerManager = powerManager;
        powerManager.continues += Continues;
        timeStarted = Time.time;

        Start();
    }

    /// <summary>
    /// Gets called once when activating
    /// </summary>
    public virtual void Start() { }

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
    ///  Gets called once every frame as long as the duration has not run out
    /// </summary>
    public virtual void Update() { }

    /// <summary>
    /// Gets called one time instead of update if the duration has run out
    /// </summary>
    public virtual void End() { }
}