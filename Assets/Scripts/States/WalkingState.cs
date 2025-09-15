using UnityEngine.InputSystem;

public class WalkingState : BasePlayerState
{
    // TODO Change movement
    public override void HandleMovement(InputAction.CallbackContext context, FloatingMovement movement)
    {
        movement.FlyingInput(context);
    }
}