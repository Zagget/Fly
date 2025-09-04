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
    public PlayerInput playerInput;

    public event Action<InputAction.CallbackContext> buttonAContext;
    public event Action<InputAction.CallbackContext> buttonBContext;
    public event Action<InputAction.CallbackContext> r_JoyStickContext;

    private void OnEnable()
    {
        if (playerInput == null)
            playerInput = GetComponent<PlayerInput>();

        r_JoyStickAction = playerInput.actions["Move"];
        lookDirection = playerInput.actions["Look"];
        r_ButtonAAction = playerInput.actions["ButtonA"];
        r_ButtonBAction = playerInput.actions["ButtonB"];

        r_JoyStickAction.performed += EventHandeler;
        r_JoyStickAction.canceled += EventHandeler;
        r_ButtonAAction.performed += EventHandeler;
        r_ButtonBAction.performed += EventHandeler;
    }

    void EventHandeler(InputAction.CallbackContext context)
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
    }

    private void OnDisable()
    {
        r_JoyStickAction.performed -= EventHandeler;
        r_ButtonAAction.performed -= EventHandeler;
        r_ButtonBAction.performed -= EventHandeler;
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