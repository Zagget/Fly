using UnityEngine;

public class BaseTogglePower : BasePower
{
    [SerializeField] protected float chargeDrain;
    bool isOn;

    public override void Activate(RigManager rigManager, float currentCharge, PowerManager powerManager, PlayerController playerController)
    {
        base.Activate(rigManager, currentCharge, powerManager, playerController);
        powerManager.continues -= Continues;

        isOn = !isOn;
        if (isOn)
        {
            powerManager.continues += Continues;
            Start();
        }
        else End();
    }

    public override void Continues()
    {
        if (RigManager.instance.legRubbing.RemoveRubbing(chargeDrain * Time.deltaTime) <= 0)
        {
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
        End();
    }
}
