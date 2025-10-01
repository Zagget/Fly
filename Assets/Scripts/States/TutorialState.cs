using UnityEngine.InputSystem;
using UnityEngine;

public class TutorialState : BasePlayerState
{
    private BasePlayerState previousState;
    private RigManager rig;
    public override void Enter(PlayerController player)
    {
        base.Enter(player);
        rig = RigManager.instance;
        StateManager.Instance.OnStateChanged += HandleStateChanged;

        if (RigManager.instance.usingVr)
        {
            player.leftPointer.SetVisible(true);
            player.rightPointer.SetVisible(true);
        }
    }

    public override void Exit()
    {
        base.Exit();

        if (RigManager.instance.usingVr)
        {
            player.leftPointer.SetVisible(false);
            player.rightPointer.SetVisible(false);
        }

        StateManager.Instance.OnStateChanged -= HandleStateChanged;
    }


    public override void HandleActivatePower(InputAction.CallbackContext context, PlayerController playerController)
    {
        if (RigManager.instance.usingVr)
        {
            player.leftPointer.OnPress(context);
        }
        else
        {
            player.desktopMenu.Press(context);
        }
    }

    public override void HandleTriggerRight(InputAction.CallbackContext context)
    {
        player.rightPointer.OnPress(context);
    }

    public override void HandleDesktopFlight(InputAction.CallbackContext context, DesktopMovement movement)
    {
        player.desktopMenu.Navigate(context);
    }


    private void HandleStateChanged(BasePlayerState newState, BasePlayerState oldState)
    {
        previousState = oldState;
    }

    public void ExitMenu()
    {
        player.SetState(previousState);
    }

    public override void StateUpdate()
    {
        rig.currentRb.linearVelocity = Vector3.zero;
    }

    public override void HandleToggleMenu(InputAction.CallbackContext context) { }
    public override void HandleMovement(InputAction.CallbackContext context, FloatingMovement movement) { }
    public override void HandleGrabLeft(InputAction.CallbackContext context, Grabber leftGrabber) { }
    public override void HandleGrabRight(InputAction.CallbackContext context, Grabber leftGrabber) { }
}