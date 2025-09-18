using UnityEngine;

public class BaseTogglePower : BasePower
{
    [SerializeField] protected float chargeDrain;
    bool isOn;

    public override void Activate(RigManager rigManager, float currentCharge, PowerManager powerManager)
    {
        base.Activate(rigManager, currentCharge, powerManager);
        powerManager.continues -= Continues;

        isOn = !isOn;
        if (isOn)
        {
            powerManager.continues += Continues;
            Start();
        }
    }

    public override void Continues()
    {
        if (LegRubbing.Instance.RemoveRubbing(chargeDrain * Time.deltaTime) <= 0)
        {
            End();
            DeactivateToggle();
        }
        else
        {
            Update();
        }
    }

    public override void DeactivateToggle() 
    {
        isOn = false;
        if (powerManager != null) powerManager.continues -= Continues; 
    }
}
