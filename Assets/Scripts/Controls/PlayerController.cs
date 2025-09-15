using System;
using UnityEngine;
using UnityEngine.InputSystem;

public enum MovementState
{
    Flying,
    Walking,
    Dash,
    None // Used for menu and testing
}

public class PlayerController : MonoBehaviour
{
    [Header("Controls")]
    [Header("Ref")]
    [SerializeField] private FloatingMovement floatingMovement;
    private DesktopMovement desktopMovement;

    [SerializeField] private LookingControls lookingControls;

    [Header("Grabbers")]
    [SerializeField] private Grabber leftGrabber;
    [SerializeField] private Grabber rightGrabber;
    [SerializeField] private Grabber desktopGrabber;

    private MovementState currentMov = MovementState.Flying;

    private bool vr;

    Input inputActions;

    private void Start()
    {
        vr = RigManager.instance.usingVr;

        inputActions = InputManager.Instance.inputActions;

        if (!vr) desktopMovement = GetComponent<DesktopMovement>();

        SubscribeToInputs();
    }

    private void SubscribeToInputs()
    {
        // Right hand
        SubscribeToAction(inputActions.RightHand.Movement, OnMove);
        SubscribeToAction(inputActions.RightHand.GrabRight, GrabRight);

        // Left hand
        SubscribeToAction(inputActions.LeftHand.Rotate, OnRotateVision);
        inputActions.LeftHand.ActivatePower.started += ActivatePower;
        inputActions.LeftHand.TogglePower.started += TogglePower;

        if (vr)
        {
            SubscribeToAction(inputActions.LeftHand.GrabLeft, GrabLeft);
            SubscribeToAction(inputActions.RightHand.GrabRight, GrabRight);
        }

        // Desktop
        inputActions.Desktop.LegRubbing.started += DesktopLegRubbing;
        SubscribeToAction(inputActions.Desktop.FlyUp, OnFlyUp);
        SubscribeToAction(inputActions.Desktop.FlyDown, OnFlyDown);
        SubscribeToAction(inputActions.Desktop.WASD, DesktopFlight);

        // Look around with mouse
        if (!vr)
        {
            SubscribeToAction(inputActions.Desktop.GrabDesktop, GrabDesktop);
            inputActions.Desktop.LegRubbing.started += DesktopLegRubbing;
            inputActions.LeftHand.MousePointer.performed += OnLook;
        }
    }

    private void OnDisable()
    {
        // Right hand
        UnsubscribeFromAction(inputActions.RightHand.Movement, OnMove);
        UnsubscribeFromAction(inputActions.RightHand.GrabRight, GrabRight);

        // Left hand
        UnsubscribeFromAction(inputActions.LeftHand.Rotate, OnRotateVision);
        inputActions.LeftHand.ActivatePower.started -= ActivatePower;
        inputActions.LeftHand.TogglePower.started -= TogglePower;

        if (vr)
        {
            UnsubscribeFromAction(inputActions.LeftHand.GrabLeft, GrabLeft);
            UnsubscribeFromAction(inputActions.RightHand.GrabRight, GrabRight);
        }

        // Desktop
        inputActions.Desktop.LegRubbing.started -= DesktopLegRubbing;
        UnsubscribeFromAction(inputActions.Desktop.FlyUp, OnFlyUp);
        UnsubscribeFromAction(inputActions.Desktop.FlyDown, OnFlyDown);
        UnsubscribeFromAction(inputActions.Desktop.WASD, DesktopFlight);

        // Look around with mouse
        if (!vr)
        {
            inputActions.Desktop.LegRubbing.started -= DesktopLegRubbing;
            inputActions.Desktop.LegRubbing.started -= DesktopLegRubbing;
            inputActions.LeftHand.MousePointer.performed -= OnLook;
        }
    }

    private void SubscribeToAction(InputAction action, Action<InputAction.CallbackContext> callback)
    {
        action.started += callback;
        action.performed += callback;
        action.canceled += callback;
    }

    private void UnsubscribeFromAction(InputAction action, Action<InputAction.CallbackContext> callback)
    {
        action.started -= callback;
        action.performed -= callback;
        action.canceled -= callback;
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        if (vr) floatingMovement.FlyingInput(context);
    }

    private void GrabLeft(InputAction.CallbackContext context)
    {
        leftGrabber.OnGrabInput(context);
    }

    private void GrabRight(InputAction.CallbackContext context)
    {
        rightGrabber.OnGrabInput(context);
    }

    private void GrabDesktop(InputAction.CallbackContext context)
    {
        desktopGrabber.OnGrabInput(context);
    }

    private void OnFlyUp(InputAction.CallbackContext context)
    {
        desktopMovement.FlyUp(context);
    }

    private void OnFlyDown(InputAction.CallbackContext context)
    {
        desktopMovement.FlyDown(context);
    }

    private void DesktopFlight(InputAction.CallbackContext context)
    {
        desktopMovement.FlyingInput(context);
    }

    private void OnLook(InputAction.CallbackContext context)
    {
        Vector2 lookDelta = context.ReadValue<Vector2>();

        lookingControls.OnLook(lookDelta);
    }

    private void ActivatePower(InputAction.CallbackContext context)
    {
        PowerManager.Instance.ActivatePower(context);
    }

    private void TogglePower(InputAction.CallbackContext context)
    {
        PowerProgression.Instance.NextPower(context);
    }

    private void DesktopLegRubbing(InputAction.CallbackContext contex)
    {
        LegRubbing.Instance.HandleDesktopRubbing(contex);
    }

    private void OnRotateVision(InputAction.CallbackContext context)
    {
        lookingControls.OnRotate(context);
    }

    private void OnAPressed(InputAction.CallbackContext context)
    {

    }

    private void OnBPressed(InputAction.CallbackContext context)
    {

    }
}