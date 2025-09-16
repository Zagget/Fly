using UnityEngine.InputSystem;

public class FlyingState : BasePlayerState
{
    public override void HandleMovement(InputAction.CallbackContext context, FloatingMovement movement)
    {
        movement.FlyingInput(context);
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
}