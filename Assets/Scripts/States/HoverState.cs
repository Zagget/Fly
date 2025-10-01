using UnityEngine;
using UnityEngine.InputSystem;

public class HoverState : BasePlayerState
{

    private RigManager rig;

    public override void Enter(PlayerController player)
    {
        base.Enter(player);
        rig = RigManager.instance;
    }
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

    public override void StateUpdate()
    {
        rig.currentRb.linearVelocity = Vector3.zero;
    }
}