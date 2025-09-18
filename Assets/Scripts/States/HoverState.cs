using UnityEngine.InputSystem;

public class HoverState : BasePlayerState
{
    public override void HandleMovement(InputAction.CallbackContext context, FloatingMovement movement)
    {
        // No movement
    }

    public override void HandleDesktopHover(InputAction.CallbackContext context)
    {
        base.HandleDesktopHover(context);

        player.SetState(StateManager.Instance.flyingState);
        StateManager.Instance.ResetHover();
    }
}