using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private static InputManager _instance;
    public static InputManager Instance { get { return _instance; } }

    private InputAction r_JoyStickAction;
    private InputAction lookDirection;
    private InputAction r_ButtonAAction;
    private InputAction r_ButtonBAction;
    private InputAction leftArrowKey;
    private InputAction rightArrowKey;
    private PlayerInput playerInput;

    public event Action<InputAction.CallbackContext> buttonAContext;
    public event Action<InputAction.CallbackContext> buttonBContext;
    public event Action<InputAction.CallbackContext> r_JoyStickContext;
    public event Action<InputAction.CallbackContext> leftArrowContext;
    public event Action<InputAction.CallbackContext> rightArrowContext;
    private void OnEnable()
    {
        if (playerInput == null)
            playerInput = GetComponent<PlayerInput>();

        r_JoyStickAction = playerInput.actions["Move"];
        lookDirection = playerInput.actions["Look"];
        r_ButtonAAction = playerInput.actions["ButtonA"];
        r_ButtonBAction = playerInput.actions["ButtonB"];
        leftArrowKey = playerInput.actions["LeftArrowKey"];
        rightArrowKey = playerInput.actions["RightArrowKey"];

        r_JoyStickAction.performed += EventHandler;
        r_JoyStickAction.canceled += EventHandler;
        r_ButtonAAction.performed += EventHandler;
        r_ButtonBAction.performed += EventHandler;
        leftArrowKey.performed += EventHandler;
        rightArrowKey.performed += EventHandler;
    }

    void EventHandler(InputAction.CallbackContext context)
    {
        if (context.action == r_ButtonAAction)
        {
            buttonAContext?.Invoke(context);
        }

        if (context.action == r_ButtonBAction)
        {
            buttonBContext?.Invoke(context);
        }

        if (context.action == r_JoyStickAction)
        {
            r_JoyStickContext?.Invoke(context);
        }

        if (context.action == leftArrowKey)
        {
            leftArrowContext?.Invoke(context);
        }

        if (context.action == rightArrowKey)
        {
            rightArrowContext?.Invoke(context);
        }
    }

    private void OnDisable()
    {
        r_JoyStickAction.performed -= EventHandler;
        r_ButtonAAction.performed -= EventHandler;
        r_ButtonBAction.performed -= EventHandler;
        leftArrowKey.performed -= EventHandler;
        rightArrowKey.performed -= EventHandler;
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}

/* Example code
 *  Read a value: Vector2 contextValue = context.ReadValue<Vector2>();
 * 
 */