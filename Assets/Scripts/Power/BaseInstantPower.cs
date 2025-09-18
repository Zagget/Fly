using UnityEngine;

public class BaseInstantPower : BasePower
{
    [SerializeField] protected float duration = 0;

    public override void Activate(RigManager rigManager, float currentCharge, PowerManager powerManager)
    {
        base.Activate(rigManager, currentCharge, powerManager);
        powerManager.continues += Continues;

        LegRubbing.Instance.RemoveRubbing(maximumCharge);
        Start();
    }

    public override void Continues()
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
}
