using System;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private DesktopMovement desktopMovement;
    [SerializeField] private LookingControls lookingControls;

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

    public static PlayerController Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
    }

    private void Start()
    {
        vr = RigManager.instance.usingVr;

        inputActions = InputManager.Instance.inputActions;

        if (!vr) desktopMovement = GetComponent<DesktopMovement>();

        if (lookingControls == null) lookingControls = GetComponent<LookingControls>();

        if (leftGrabber == null) Debug.LogWarning("LeftGrabber is empty");
        if (rightGrabber == null) Debug.LogWarning("RightGrabber is empty");
        if (desktopGrabber == null) Debug.LogWarning("DesktopGrabber is empty");

        SubscribeToInputs();

        SetState(StateManager.Instance.flyingState);
    }

    private void SubscribeToInputs()
    {
        if (vr)
        {
            // Right hand
            SubscribeToAction(inputActions.RightHand.GrabRight, GrabRight);
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

            SubscribeToAction(inputActions.Desktop.GrabDesktop, OnGrabDesktop);
            inputActions.Desktop.LegRubbing.started += OnLegRubbingDesktop;
            inputActions.Desktop.MousePointer.performed += OnLookDesktop;
        }

        SubscribeToAction(inputActions.RightHand.Menu, ToggleMenu);
        LegRubbing.Instance.chargeChange += RubbingChange;
    }

    private void OnDisable()
    {
        if (vr)
        {
            // Right hand
            UnsubscribeFromAction(inputActions.RightHand.GrabRight, GrabRight);
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

        UnsubscribeFromAction(inputActions.RightHand.Menu, ToggleMenu);
        LegRubbing.Instance.chargeChange -= RubbingChange;
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

    private void GrabRight(InputAction.CallbackContext context) => currentState?.HandleGrabRight(context, rightGrabber);
    private void GrabLeft(InputAction.CallbackContext context) => currentState?.HandleGrabLeft(context, leftGrabber);
    private void OnRotateVision(InputAction.CallbackContext context) => currentState?.HandleRotateVision(context, lookingControls);
    private void ToggleMenu(InputAction.CallbackContext context) => currentState?.HandleToggleMenu(context);
    private void OnTriggerRight(InputAction.CallbackContext context) => currentState?.HandleTriggerRight(context);
    private void RubbingChange(float amount) => currentState?.HandleRubbingChange(amount);

    // Desktop
    private void OnLookDesktop(InputAction.CallbackContext context) => currentState?.HandleDesktopLook(context, lookingControls);
    private void DesktopFlight(InputAction.CallbackContext context) => currentState?.HandleDesktopFlight(context, desktopMovement);
    private void OnFlyUpDesktop(InputAction.CallbackContext context) => currentState?.HandleDesktopFlyUp(context, desktopMovement);
    private void OnFlyDownDesktop(InputAction.CallbackContext context) => currentState?.HandleDesktopFlyDown(context, desktopMovement);
    private void OnGrabDesktop(InputAction.CallbackContext context) => currentState?.HandleDesktopGrab(context, desktopGrabber);
    private void OnLegRubbingDesktop(InputAction.CallbackContext context) => currentState?.HandleDesktopLegRubbing(context);
}