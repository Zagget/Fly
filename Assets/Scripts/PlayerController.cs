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

    private void Start()
    {
        vr = RigManager.instance.usingVr;

        SubscribeToInputs();
    }

    private void SubscribeToInputs()
    {
        // Movement 
        InputManager.Instance.r_JoyStickAction.performed += OnMove;
        InputManager.Instance.r_JoyStickAction.canceled += OnMoveCanceled;
        InputManager.Instance.rotateVisionAction.performed += OnRotateVision;
        InputManager.Instance.flyUpAction.performed += FlyUp;
        InputManager.Instance.flyUpAction.canceled += FlyUp;

        InputManager.Instance.flyDownAction.performed += FlyDown;
        InputManager.Instance.flyDownAction.canceled += FlyDown;

        if (!vr)
        {
            InputManager.Instance.lookDirection.performed += OnLook;
        }

        // Buttons
        InputManager.Instance.r_ButtonAAction.performed += OnAPressed;
        InputManager.Instance.r_ButtonBAction.performed += OnBPressed;
    }

    private void OnDisable()
    {
        // Movement
        InputManager.Instance.r_JoyStickAction.performed -= OnMove;
        InputManager.Instance.rotateVisionAction.performed -= OnRotateVision;
        InputManager.Instance.flyUpAction.performed -= FlyUp;
        InputManager.Instance.flyUpAction.canceled -= FlyUp;

        InputManager.Instance.flyDownAction.performed -= FlyDown;
        InputManager.Instance.flyDownAction.canceled -= FlyDown;

        if (!vr)
        {
            InputManager.Instance.lookDirection.performed -= OnLook;
        }

        // Buttons
        InputManager.Instance.r_ButtonAAction.performed -= OnAPressed;
        InputManager.Instance.r_ButtonBAction.performed -= OnBPressed;
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        Vector2 moveInput = context.ReadValue<Vector2>();

        Debug.Log($"blä MoveStarted");
        switch (currentMov)
        {
            case MovementState.Flying:
                flightControls.FlyingInput(moveInput);

                break;

            case MovementState.Walking:

                break;
        }
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        Debug.Log($"blä move canceled");
    }

    private void FlyUp(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            flightControls.FlyUp(true);
        }
        else
        {
            flightControls.FlyUp(false);
        }
    }

    private void FlyDown(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            flightControls.FlyDown(true);
        }
        else
        {
            flightControls.FlyDown(false);
        }
    }

    private void OnLook(InputAction.CallbackContext context)
    {
        Vector2 lookDelta = context.ReadValue<Vector2>();

        lookingControls.OnLook(lookDelta);
    }

    private void OnRotateVision(InputAction.CallbackContext context)
    {
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