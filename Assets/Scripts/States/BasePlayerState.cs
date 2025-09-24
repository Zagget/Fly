using UnityEngine;
using UnityEngine.InputSystem;

public abstract class BasePlayerState
{
    protected PlayerController player;
    public virtual void Enter(PlayerController player)
    {
        this.player = player;
    }

    public virtual void Exit() { }
    /// <summary>
    /// Update but only gets triggered when current state is active.
    /// </summary>
    public virtual void StateUpdate() { }

    // Add walking script to param
    public virtual void HandleMovement(InputAction.CallbackContext context, FloatingMovement floatingMovement) { }

    public virtual void HandleWalkingMovement(InputAction.CallbackContext context, WalkingMovement walkingMovement) { }

    public virtual void HandlePrimaryButton(InputAction.CallbackContext context) { }
    public virtual void HandleRotateVision(InputAction.CallbackContext context, LookingControls looking)
    {
        looking.OnRotate(context);
    }

    public virtual void HandleGrabLeft(InputAction.CallbackContext context, Grabber leftGrabber)
    {
        leftGrabber.OnGrabInput(context);
    }

    public virtual void HandleGrabRight(InputAction.CallbackContext context, Grabber rightGrabber)
    {
        rightGrabber.OnGrabInput(context);
    }

    public virtual void HandleActivatePower(InputAction.CallbackContext context, PlayerController playerController)
    {
        PowerManager.Instance.ActivatePower(context, playerController);
    }

    public virtual void HandleTogglePower(InputAction.CallbackContext context)
    {
        PowerProgression.Instance.NextPower(context);
    }

    public virtual void HandleToggleMenu(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            player.SetState(StateManager.Instance.menuState);
            Debug.Log("State started");
        }
    }

    public virtual void HandleTriggerRight(InputAction.CallbackContext context)
    {
    }

    // Desktop Inputs
    public virtual void HandleDesktopGrab(InputAction.CallbackContext context, Grabber desktopGrabber)
    {
        desktopGrabber.OnGrabInput(context);
    }

    public virtual void HandleDesktopLook(InputAction.CallbackContext context, LookingControls looking)
    {
        Vector2 lookDelta = context.ReadValue<Vector2>();

        looking.OnLook(lookDelta);
    }

    public virtual void HandleDesktopLegRubbing(InputAction.CallbackContext context)
    {
        LegRubbing.Instance.HandleDesktopRubbing(context);
    }

    public virtual void HandleDesktopFlyUp(InputAction.CallbackContext context, DesktopMovement movement)
    {
        movement.FlyUp(context);
    }

    public virtual void HandleDesktopFlyDown(InputAction.CallbackContext context, DesktopMovement movement)
    {
        movement.FlyDown(context);
    }
    public virtual void HandleDesktopFlight(InputAction.CallbackContext context, DesktopMovement movement)
    {
        movement.FlyingInput(context);
    }

    public virtual void HandleDesktopHover(InputAction.CallbackContext context)
    {

    }
}