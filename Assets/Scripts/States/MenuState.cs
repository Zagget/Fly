using System.Diagnostics;
using UnityEngine.InputSystem;
using UnityEngine;

public class MenuState : BasePlayerState
{
    private BasePlayerState previousState;

    public override void Enter(PlayerController player)
    {
        base.Enter(player);
        StateManager.Instance.OnStateChanged += HandleStateChanged;

        player.menu.EnterMenu();

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

    public override void HandleMovement(InputAction.CallbackContext context, FloatingMovement movement)
    {
        //ToDO Select in menu as well
    }

    public override void HandleActivatePower(InputAction.CallbackContext context, PlayerController playerController)
    {
        if (!RigManager.instance.usingVr)
            player.desktopMenu.Press(context);
    }

    public override void HandleDesktopFlight(InputAction.CallbackContext context, DesktopMovement movement)
    {
        player.desktopMenu.Navigate(context);
    }

    public override void HandleGrabLeft(InputAction.CallbackContext context, Grabber leftGrabber)
    {
        player.leftPointer.OnPress(context);
    }

    public override void HandleGrabRight(InputAction.CallbackContext context, Grabber leftGrabber)
    {
        player.rightPointer.OnPress(context);
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
}