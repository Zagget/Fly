using System;
using Meta.XR.InputActions;
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
    [Header("Ref")]
    [SerializeField] private FloatingMovement flightControls; //TODO make sure this is correct class.
    [SerializeField] private LookingControls lookingControls;

    private MovementState currentMov = MovementState.Flying;

    private bool vr;

    Input inputActions;

    private void Start()
    {
        vr = RigManager.instance.usingVr;

        inputActions = InputManager.Instance.inputActions;

        SubscribeToInputs();
    }

    private void SubscribeToInputs()
    {
        // Right hand
        SubscribeToAction(inputActions.RightHand.Movement, OnMove);
        //SubscribeToAction(inputActions.RightHand.FlyUp, OnFlyUp);
        //SubscribeToAction(inputActions.RightHand.FlyDown, OnFlyDown);

        // Left hand
        SubscribeToAction(inputActions.LeftHand.Rotate, OnRotateVision);
        SubscribeToAction(inputActions.LeftHand.ActivatePower, ActivatePower);
        inputActions.LeftHand.TogglePower.started += TogglePower;

        // Desktop
        inputActions.Desktop.LegRubbing.started += DesktopLegRubbing;

        // Look around with mouse
        if (!vr)
        {
            inputActions.LeftHand.MousePointer.performed += OnLook;
        }
    }


    private void OnDisable()
    {
        // Right hand
        UnsubscribeFromAction(inputActions.RightHand.Movement, OnMove);
        //UnsubscribeFromAction(inputActions.RightHand.FlyUp, OnFlyUp);
        //UnsubscribeFromAction(inputActions.RightHand.FlyDown, OnFlyDown);

        // Left hand
        UnsubscribeFromAction(inputActions.LeftHand.Rotate, OnRotateVision);
        UnsubscribeFromAction(inputActions.LeftHand.ActivatePower, ActivatePower);
        inputActions.LeftHand.TogglePower.started -= TogglePower;

        // Desktop
        inputActions.Desktop.LegRubbing.started -= DesktopLegRubbing;

        // Look around with mouse
        if (!vr)
        {
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
        flightControls.FlyingInput(context);
    }


    //private void OnFlyUp(InputAction.CallbackContext context)
    //{
    //    flightControls.FlyUp(context);
    //}

    //private void OnFlyDown(InputAction.CallbackContext context)
    //{
    //    flightControls.FlyDown(context);
    //}

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