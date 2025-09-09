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
    [SerializeField] private FlightControls flightControls;
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
        inputActions.RightHand.Movement.started += OnMove;
        inputActions.RightHand.Movement.performed += OnMove;
        inputActions.RightHand.Movement.canceled += OnMove;

        inputActions.RightHand.FlyUp.started += OnFlyUp;
        inputActions.RightHand.FlyUp.performed += OnFlyUp;
        inputActions.RightHand.FlyUp.canceled += OnFlyUp;

        inputActions.RightHand.FlyDown.started += OnFlyDown;
        inputActions.RightHand.FlyDown.performed += OnFlyDown;
        inputActions.RightHand.FlyDown.canceled += OnFlyDown;

        // Left Hand
        inputActions.LeftHand.Rotate.started += OnRotateVision;
        inputActions.LeftHand.Rotate.performed += OnRotateVision;
        inputActions.LeftHand.Rotate.canceled += OnRotateVision;

        // Look around with mouse
        if (!vr)
        {
            inputActions.LeftHand.MousePointer.performed += OnLook;
        }
    }


    private void OnDisable()
    {

    }

    private void OnMove(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.Log($"blä Move Started");

        }

        if (context.performed)
        {
            Debug.Log($"blä Move Performed");

        }

        if (context.canceled)
        {
            Debug.Log($"blä move canceled");
        }


    }


    private void OnFlyUp(InputAction.CallbackContext context)
    {
        // float triggerValue = context.ReadValue<float>();
        // Debug.Log($"blä FlyUp value: {triggerValue}");

        if (context.started)
        {
            Debug.Log($"blä FlyUp Started");

        }

        if (context.performed)
        {
            Debug.Log($"blä FlyUp Performed");

        }

        if (context.canceled)
        {
            Debug.Log($"blä FlyUp canceled");
        }
    }

    private void OnFlyDown(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.Log($"blä FlyDown Started");

        }

        if (context.performed)
        {
            Debug.Log($"blä FlyDown Performed");

        }

        if (context.canceled)
        {
            Debug.Log($"blä FlyDown canceled");
        }
    }

    private void OnLook(InputAction.CallbackContext context)
    {
        Vector2 lookDelta = context.ReadValue<Vector2>();

        lookingControls.OnLook(lookDelta);
    }

    private void OnRotateVision(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.Log($"blä Rotate Started");

        }

        if (context.performed)
        {
            Debug.Log($"blä Rotate Performed");

        }

        if (context.canceled)
        {
            Debug.Log($"blä Rotate canceled");
        }
        Vector2 rotateInput = context.ReadValue<Vector2>();

        StartCoroutine(lookingControls.RotateVision(rotateInput));
    }

    private void OnAPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {

        }
    }

    private void OnBPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {

        }
    }
}