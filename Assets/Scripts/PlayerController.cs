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

    InputNewVR inputActions;

    private void Start()
    {
        vr = RigManager.instance.usingVr;

        inputActions = InputManager.Instance.inputActions;

        SubscribeToInputs();
    }

    private void SubscribeToInputs()
    {
        // Movement 
        inputActions.ActionMap.Movement.performed += OnMove;
        inputActions.ActionMap.Movement.canceled += OnMove;
        inputActions.ActionMap.Movement.started += OnMove;

        //InputManager.Instance.rotateVisionAction.performed += OnRotateVision;
        //InputManager.Instance.flyUpAction.performed += FlyUp;
        //InputManager.Instance.flyUpAction.canceled += FlyUp;

        //InputManager.Instance.flyDownAction.performed += FlyDown;
        //InputManager.Instance.flyDownAction.canceled += FlyDown;

        //if (!vr)
        //{
        //    InputManager.Instance.lookDirection.performed += OnLook;
        //}

        //// Buttons
        //InputManager.Instance.r_ButtonAAction.performed += OnAPressed;
        //InputManager.Instance.r_ButtonBAction.performed += OnBPressed;



        // Subscribe to the button press
        inputActions.ActionMap.RightShoot.canceled += FlyUp;
        inputActions.ActionMap.RightShoot.performed += FlyUp;
        inputActions.ActionMap.RightShoot.started += FlyUp;

        // Subscribe to the button press
        //inputActions.ActionMap.LeftShoot.canceled += LeftTrigger;
        //inputActions.ActionMap.LeftShoot.performed += LeftTrigger;
        //inputActions.ActionMap.LeftShoot.started += LeftTrigger;
    }


    private void OnDisable()
    {
        // Movement
        //InputManager.Instance.rotateVisionAction.performed -= OnRotateVision;
        //InputManager.Instance.flyUpAction.performed -= FlyUp;
        //InputManager.Instance.flyUpAction.canceled -= FlyUp;

        //InputManager.Instance.flyDownAction.performed -= FlyDown;
        //InputManager.Instance.flyDownAction.canceled -= FlyDown;

        //if (!vr)
        //{
        //    InputManager.Instance.lookDirection.performed -= OnLook;
        //}

        //// Buttons
        //InputManager.Instance.r_ButtonAAction.performed -= OnAPressed;
        //InputManager.Instance.r_ButtonBAction.performed -= OnBPressed;

        inputActions.ActionMap.Movement.performed -= OnMove;
        inputActions.ActionMap.Movement.canceled -= OnMove;
        inputActions.ActionMap.Movement.started -= OnMove;

        inputActions.ActionMap.RightShoot.canceled -= FlyUp;
        inputActions.ActionMap.RightShoot.performed -= FlyUp;
        inputActions.ActionMap.RightShoot.started -= FlyUp;

        // Subscribe to the button press
        //inputActions.ActionMap.LeftShoot.performed -= LeftTrigger;
        //inputActions.ActionMap.LeftShoot.canceled -= LeftTrigger;
        //inputActions.ActionMap.LeftShoot.started -= LeftTrigger;

        // Enable the whole action map
        inputActions.ActionMap.Disable();
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
            Debug.Log("FlyUp" + context);
            flightControls.FlyUp(true);
        }
        else if (context.canceled)
        {
            Debug.Log("StopFlyUp" + context);
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