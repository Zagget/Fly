using UnityEngine;
using UnityEngine.InputSystem;

public class MenuState : BasePlayerState
{
    private BasePlayerState oldState;

    public override void Enter(PlayerController player)
    {
        base.Enter(player);
        StateManager.Instance.OnStateChanged += HandleStateChanged;
    }

    public override void Exit()
    {
        base.Exit();
        StateManager.Instance.OnStateChanged -= HandleStateChanged;
    }

    public override void HandleMovement(InputAction.CallbackContext context, FloatingMovement movement)
    {
        //ToDO Select in menu as well
    }

    public override void HandleGrabLeft(InputAction.CallbackContext context, Grabber leftGrabber)
    {
        player.leftPointer.OnPress(context);
    }

    public override void HandleGrabRight(InputAction.CallbackContext context, Grabber leftGrabber)
    {
        player.rightPointer.OnPress(context);
    }

    private void HandleStateChanged(BasePlayerState oldState, BasePlayerState newState)
    {
        this.oldState = oldState;
    }

    public override void HandleToggleMenu(InputAction.CallbackContext context)
    {
        player.SetState(oldState);
    }
}