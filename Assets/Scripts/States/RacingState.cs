using UnityEngine;

public class RacingState : FlyingState
{
    public override void StateUpdate()
    {
        if (RigManager.instance.legRubbing.RemoveRubbing(Time.deltaTime) <= 0)
        {
            player.SetState(StateManager.Instance.flyingState);
        }
    }
}
