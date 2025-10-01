using System.Diagnostics;
using UnityEngine.InputSystem;
using UnityEngine;

public class MenuState : BasePlayerState
{
    private BasePlayerState previousState;
    private RigManager rig;
    public override void Enter(PlayerController player)
    {
        base.Enter(player);
        rig = RigManager.instance;
        StateManager.Instance.OnStateChanged += HandleStateChanged;

        player.menu.EnterMenu();

        if (rig.usingVr)
        {
            player.leftPointer.SetVisible(true);
            player.rightPointer.SetVisible(true);
        }
    }

    public override void Exit()
    {
        base.Exit();

        if (rig.usingVr)
        {
            player.leftPointer.SetVisible(false);
            player.rightPointer.SetVisible(false);
        }

        StateManager.Instance.OnStateChanged -= HandleStateChanged;
    }


    public override void HandleActivatePower(InputAction.CallbackContext context, PlayerController playerController)
    {
        if (rig.usingVr)
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

    public override void HandleToggleMenu(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            ExitMenu();
        }
    }

    public void ExitMenu()
    {
        player.menu.TogglePanels(false, false);
        player.SetState(previousState);
    }

    public override void StateUpdate()
    {
        base.StateUpdate();
        rig.currentRb.linearVelocity = Vector3.zero;
    }
    public override void HandleMovement(InputAction.CallbackContext context, FloatingMovement movement) { }
    public override void HandleGrabLeft(InputAction.CallbackContext context, Grabber leftGrabber) { }
    public override void HandleGrabRight(InputAction.CallbackContext context, Grabber leftGrabber) { }
}