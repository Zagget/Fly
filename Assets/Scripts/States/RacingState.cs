using UnityEngine;

public class RacingState : FlyingState
{
    public override void StateUpdate()
    {
        if (LegRubbing.Instance.RemoveRubbing(Time.deltaTime) <= 0)
        {
            player.SetState(StateManager.Instance.flyingState);
        }
    }
}
