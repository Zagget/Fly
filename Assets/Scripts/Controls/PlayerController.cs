using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private FloatingMovement floatingMovement;
    [SerializeField] private DesktopMovement desktopMovement;
    [SerializeField] private LookingControls lookingControls;

    [Header("Grabbers")]
    [SerializeField] private Grabber leftGrabber;
    [SerializeField] private Grabber rightGrabber;
    [SerializeField] private Grabber desktopGrabber;

    public readonly BasePlayerState flyingState = new FlyingState();
    public readonly BasePlayerState hoverState = new HoverState();
    public readonly BasePlayerState walkingState = new WalkingState();

    private bool vr;
    private Input inputActions;
    private BasePlayerState currentState;

    private void Start()
    {
        vr = RigManager.instance.usingVr;

        inputActions = InputManager.Instance.inputActions;

        if (!vr) desktopMovement = GetComponent<DesktopMovement>();

        if (vr) floatingMovement = GetComponent<FloatingMovement>();

        if (lookingControls == null) lookingControls = GetComponent<LookingControls>();

        if (leftGrabber == null) Debug.LogWarning("LeftGrabber is empty");
        if (rightGrabber == null) Debug.LogWarning("RightGrabber is empty");
        if (desktopGrabber == null) Debug.LogWarning("DesktopGrabber is empty");

        SubscribeToInputs();

        SetState(flyingState);
    }

    private void SubscribeToInputs()
    {
        if (vr)
        {
            // Right hand
            SubscribeToAction(inputActions.RightHand.Movement, OnMove);
            SubscribeToAction(inputActions.RightHand.GrabRight, GrabRight);

            // Left hand
            SubscribeToAction(inputActions.LeftHand.GrabLeft, GrabLeft);
            SubscribeToAction(inputActions.LeftHand.Rotate, OnRotateVision);

            inputActions.LeftHand.ActivatePower.started += ActivatePower;
            inputActions.LeftHand.TogglePower.started += TogglePower;
        }
        else
        {
            SubscribeToAction(inputActions.Desktop.WASD, DesktopFlight);
            SubscribeToAction(inputActions.Desktop.Rotate, OnRotateVision);
            SubscribeToAction(inputActions.Desktop.FlyUp, OnFlyUpDesktop);
            SubscribeToAction(inputActions.Desktop.FlyDown, OnFlyDownDesktop);

            SubscribeToAction(inputActions.Desktop.GrabDesktop, OnGrabDesktop);
            inputActions.Desktop.LegRubbing.started += OnLegRubbingDesktop;
            inputActions.Desktop.MousePointer.performed += OnLookDesktop;
        }
    }

    private void OnDisable()
    {
        if (vr)
        {
            // Right hand
            UnsubscribeFromAction(inputActions.RightHand.Movement, OnMove);
            UnsubscribeFromAction(inputActions.RightHand.GrabRight, GrabRight);

            // Left hand
            UnsubscribeFromAction(inputActions.LeftHand.GrabLeft, GrabLeft);
            UnsubscribeFromAction(inputActions.LeftHand.Rotate, OnRotateVision);

            inputActions.LeftHand.ActivatePower.started -= ActivatePower;
            inputActions.LeftHand.TogglePower.started -= TogglePower;
        }
        else
        {
            UnsubscribeFromAction(inputActions.Desktop.WASD, DesktopFlight);
            UnsubscribeFromAction(inputActions.Desktop.FlyUp, OnFlyUpDesktop);
            UnsubscribeFromAction(inputActions.Desktop.FlyDown, OnFlyDownDesktop);

            UnsubscribeFromAction(inputActions.Desktop.GrabDesktop, OnGrabDesktop);
            inputActions.Desktop.LegRubbing.started -= OnLegRubbingDesktop;
            inputActions.Desktop.MousePointer.performed -= OnLookDesktop;
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

    public void SetState(BasePlayerState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState?.Enter(this);
    }

    public BasePlayerState GetState()
    {
        return currentState;
    }

    private void Update()
    {
        currentState?.Update();
    }

    private void OnMove(InputAction.CallbackContext context) => currentState?.HandleMovement(context, floatingMovement);
    private void GrabRight(InputAction.CallbackContext context) => currentState?.HandleGrabRight(context, rightGrabber);
    private void GrabLeft(InputAction.CallbackContext context) => currentState?.HandleGrabLeft(context, leftGrabber);
    private void OnRotateVision(InputAction.CallbackContext context) => currentState?.HandleRotateVision(context, lookingControls);
    private void ActivatePower(InputAction.CallbackContext context) => currentState?.HandleActivatePower(context);
    private void TogglePower(InputAction.CallbackContext context) => currentState?.HandleTogglePower(context);

    // Desktop
    private void OnLookDesktop(InputAction.CallbackContext context) => currentState?.HandleDesktopLook(context, lookingControls);
    private void DesktopFlight(InputAction.CallbackContext context) => currentState?.HandleDesktopFlight(context, desktopMovement);
    private void OnFlyUpDesktop(InputAction.CallbackContext context) => currentState?.HandleDesktopFlyUp(context, desktopMovement);
    private void OnFlyDownDesktop(InputAction.CallbackContext context) => currentState?.HandleDesktopFlyDown(context, desktopMovement);
    private void OnGrabDesktop(InputAction.CallbackContext context) => currentState?.HandleDesktopGrab(context, desktopGrabber);
    private void OnLegRubbingDesktop(InputAction.CallbackContext context) => currentState?.HandleDesktopLegRubbing(context);
}