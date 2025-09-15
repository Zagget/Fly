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
    public virtual void Update() { }

    // Add walking script to param
    public virtual void HandleMovement(InputAction.CallbackContext context, FloatingMovement floatingMovement) { }

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

    public virtual void HandleActivatePower(InputAction.CallbackContext context)
    {
        PowerManager.Instance.ActivatePower(context);
    }

    public virtual void HandleTogglePower(InputAction.CallbackContext context)
    {
        PowerProgression.Instance.NextPower(context);
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
}