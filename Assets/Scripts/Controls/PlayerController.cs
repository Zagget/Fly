using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private FloatingMovement floatingMovement;
    [SerializeField] private DesktopMovement desktopMovement;
    [SerializeField] private LookingControls lookingControls;
    private WalkingMovement walkingMovement;

    [Header("Grabbers")]
    [SerializeField] public Grabber leftGrabber;
    [SerializeField] public Grabber rightGrabber;
    [SerializeField] public Grabber desktopGrabber;
    [SerializeField] public UIPointer leftPointer;
    [SerializeField] public UIPointer rightPointer;

    [Header("Menu")]
    [SerializeField] public TutorialUI tutorial;
    [SerializeField] public MenuUI menu;
    [SerializeField] public DesktopMenu desktopMenu;

    private bool vr;
    private Input inputActions;
    private BasePlayerState currentState;

    private void Start()
    {
        vr = RigManager.instance.usingVr;

        inputActions = InputManager.Instance.inputActions;

        if (!vr) desktopMovement = GetComponent<DesktopMovement>();

        if (vr) floatingMovement = GetComponent<FloatingMovement>();

        if (vr) walkingMovement = GetComponent<WalkingMovement>();

        if (lookingControls == null) lookingControls = GetComponent<LookingControls>();

        if (leftGrabber == null) Debug.LogWarning("LeftGrabber is empty");
        if (rightGrabber == null) Debug.LogWarning("RightGrabber is empty");
        if (desktopGrabber == null) Debug.LogWarning("DesktopGrabber is empty");

        SubscribeToInputs();

        SetState(StateManager.Instance.walkingState);
    }

    private void SubscribeToInputs()
    {
        if (vr)
        {
            // Right hand
            SubscribeToAction(inputActions.RightHand.Movement, OnMove);
            SubscribeToAction(inputActions.RightHand.GrabRight, GrabRight);
            SubscribeToAction(inputActions.RightHand.Movement, OnGroundMove);
            SubscribeToPressed(inputActions.RightHand.Hover, OnHoverPressed);
            SubscribeToPressed(inputActions.RightHand.Hover, OnButtonAPressed);
            SubscribeToPressed(inputActions.RightHand.TriggerButton, OnTriggerRight);


            // Left hand
            SubscribeToAction(inputActions.LeftHand.GrabLeft, GrabLeft);
            SubscribeToAction(inputActions.LeftHand.Rotate, OnRotateVision);
        }
        else
        {
            SubscribeToAction(inputActions.Desktop.WASD, DesktopFlight);
            SubscribeToAction(inputActions.Desktop.Rotate, OnRotateVision);
            SubscribeToAction(inputActions.Desktop.FlyUp, OnFlyUpDesktop);
            SubscribeToAction(inputActions.Desktop.FlyDown, OnFlyDownDesktop);
            SubscribeToPressed(inputActions.Desktop.Hover, OnHoverPressed);

            SubscribeToAction(inputActions.Desktop.GrabDesktop, OnGrabDesktop);
            inputActions.Desktop.LegRubbing.started += OnLegRubbingDesktop;
            inputActions.Desktop.MousePointer.performed += OnLookDesktop;
        }

        inputActions.LeftHand.ActivatePower.started += ActivatePower;
        inputActions.LeftHand.TogglePower.started += TogglePower;

        SubscribeToAction(inputActions.RightHand.Menu, ToggleMenu);
    }

    private void OnDisable()
    {
        if (vr)
        {
            // Right hand
            UnsubscribeFromAction(inputActions.RightHand.Movement, OnMove);
            UnsubscribeFromAction(inputActions.RightHand.GrabRight, GrabRight);
            UnsubscribeFromAction(inputActions.RightHand.Movement, OnGroundMove);
            UnsubscribeToPressed(inputActions.RightHand.Hover, OnHoverPressed);
            UnsubscribeToPressed(inputActions.RightHand.TriggerButton, OnTriggerRight);

            // Left hand
            UnsubscribeFromAction(inputActions.LeftHand.GrabLeft, GrabLeft);
            UnsubscribeFromAction(inputActions.LeftHand.Rotate, OnRotateVision);
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

        inputActions.LeftHand.ActivatePower.started -= ActivatePower;
        inputActions.LeftHand.TogglePower.started -= TogglePower;
        UnsubscribeFromAction(inputActions.RightHand.Menu, ToggleMenu);
    }

    private void SubscribeToPressed(InputAction action, Action<InputAction.CallbackContext> callback)
    {
        action.started += callback;
    }

    private void UnsubscribeToPressed(InputAction action, Action<InputAction.CallbackContext> callback)
    {
        action.started -= callback;
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
        var lastState = currentState;
        currentState?.Exit();
        currentState = newState;
        currentState?.Enter(this);
        StateManager.Instance.TriggerChangeStateEvent(newState, lastState);
        Debug.Log($"new state {newState} old state {lastState}");
    }

    public BasePlayerState GetState()
    {
        return currentState;
    }

    private void Update()
    {
        currentState?.StateUpdate();
    }

    private void OnMove(InputAction.CallbackContext context) => currentState?.HandleMovement(context, floatingMovement);
    private void OnGroundMove(InputAction.CallbackContext context) => currentState?.HandleWalkingMovement(context, walkingMovement);
    private void GrabRight(InputAction.CallbackContext context) => currentState?.HandleGrabRight(context, rightGrabber);
    private void GrabLeft(InputAction.CallbackContext context) => currentState?.HandleGrabLeft(context, leftGrabber);
    private void OnRotateVision(InputAction.CallbackContext context) => currentState?.HandleRotateVision(context, lookingControls);
    private void ActivatePower(InputAction.CallbackContext context) => currentState?.HandleActivatePower(context, this);
    private void TogglePower(InputAction.CallbackContext context) => currentState?.HandleTogglePower(context);
    private void ToggleMenu(InputAction.CallbackContext context) => currentState?.HandleToggleMenu(context);
    private void OnButtonAPressed(InputAction.CallbackContext context) => currentState?.HandlePrimaryButton(context);
    private void OnTriggerRight(InputAction.CallbackContext context) => currentState?.HandleTriggerRight(context);

    // Desktop
    private void OnLookDesktop(InputAction.CallbackContext context) => currentState?.HandleDesktopLook(context, lookingControls);
    private void DesktopFlight(InputAction.CallbackContext context) => currentState?.HandleDesktopFlight(context, desktopMovement);
    private void OnFlyUpDesktop(InputAction.CallbackContext context) => currentState?.HandleDesktopFlyUp(context, desktopMovement);
    private void OnFlyDownDesktop(InputAction.CallbackContext context) => currentState?.HandleDesktopFlyDown(context, desktopMovement);
    private void OnGrabDesktop(InputAction.CallbackContext context) => currentState?.HandleDesktopGrab(context, desktopGrabber);
    private void OnLegRubbingDesktop(InputAction.CallbackContext context) => currentState?.HandleDesktopLegRubbing(context);
    private void OnHoverPressed(InputAction.CallbackContext context) => currentState?.HandleDesktopHover(context);
}