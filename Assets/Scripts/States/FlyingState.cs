using UnityEngine;
using UnityEngine.InputSystem;
public class FlyingState : BasePlayerState
{
    private float stateActiveTime;
    private float timeUntilCanLand = 1.5f;

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

    public override void Enter(PlayerController player)
    {
        base.Enter(player);
        stateActiveTime = 0;
    }

    public override void StateUpdate()
    {
        stateActiveTime += Time.deltaTime; 

        if (StateManager.Instance.CheckHoverState()) 
        {
            player.SetState(StateManager.Instance.hoverState);
        }
        else if (stateActiveTime > timeUntilCanLand && StateManager.Instance.CheckWalkingState())
        {
            player.SetState(StateManager.Instance.walkingState);
        }
    }
}