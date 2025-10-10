using UnityEngine;

public class BaseInstantPower : BasePower
{
    [SerializeField] protected float duration = 0;

    public override void Activate(RigManager rigManager, float currentCharge, PowerManager powerManager, PlayerController playerController)
    {
        base.Activate(rigManager, currentCharge, powerManager, playerController);
        powerManager.continues += Continues;

        RigManager.instance.legRubbing.RemoveRubbing(maximumCharge);
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
