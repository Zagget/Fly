using UnityEngine;
using UnityEngine.InputSystem;
public class InputManager : MonoBehaviour
{
    private static InputManager _instance;
    public static InputManager Instance { get { return _instance; } }

    public InputAction r_JoyStickAction;
    public InputAction lookDirection;
    public InputAction r_ButtonAAction;
    public InputAction r_ButtonBAction;
    public InputAction leftArrowKey;
    public InputAction rightArrowKey;
    public InputAction flyUpAction;
    public InputAction flyDownAction;

    private PlayerInput playerInput;
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
        flyUpAction = playerInput.actions["FlyUp"];
        flyDownAction = playerInput.actions["FlyDown"];
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