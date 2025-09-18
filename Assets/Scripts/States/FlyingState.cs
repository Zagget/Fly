using UnityEngine.InputSystem;
public class FlyingState : BasePlayerState
{
    public override void HandleMovement(InputAction.CallbackContext context, FloatingMovement movement)
    {
        
    }

    // Grabbing when flying?
    public override void HandleGrabLeft(InputAction.CallbackContext context, Grabber leftGrabber)
    {
        base.HandleGrabLeft(context, leftGrabber);
    }

    public override void HandleGrabRight(InputAction.CallbackContext context, Grabber leftGrabber)
    {
        base.HandleGrabLeft(context, leftGrabber);
    }

    public override void HandleDesktopHover(InputAction.CallbackContext context)
    {
        base.HandleDesktopHover(context);

        player.SetState(StateManager.Instance.hoverState);
    }

    public override void StateUpdate()
    {
        if (StateManager.Instance.CheckHoverState()) 
        {
            player.SetState(StateManager.Instance.hoverState);
        }
        else if (StateManager.Instance.CheckWalkingState())
        {
            player.SetState(StateManager.Instance.walkingState);
        }
    }
}