using UnityEngine.InputSystem;
public class WalkingState : BasePlayerState
{
    // TODO Change movement
    public override void HandleMovement(InputAction.CallbackContext context, FloatingMovement movement)
    {
        
    }

    public override void StateUpdate()
    {
        if (StateManager.Instance.CheckFlyingState())
        {
            player.SetState(StateManager.Instance.flyingState);
        }
    }
}