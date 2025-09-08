using UnityEngine;
using UnityEngine.InputSystem;
public class InputManager : MonoBehaviour
{
    private static InputManager _instance;
    public static InputManager Instance { get { return _instance; } }

    [Header("References")]
    public PlayerInput playerInput;

    // Right hand actions
    public InputAction r_JoyStickAction;
    public InputAction r_ButtonAAction;
    public InputAction r_ButtonBAction;
    public InputAction flyUpAction;
    public InputAction flyDownAction;

    // Left hand actions
    public InputAction lookDirection;
    public InputAction rotateVisionAction;
    public InputAction activatePower;

    private void Start()
    {
        // Right hand map
        var rightMap = playerInput.actions.FindActionMap("RightHand", true);
        r_JoyStickAction = rightMap["Move"];
        r_ButtonAAction = rightMap["ButtonA"];
        r_ButtonBAction = rightMap["ButtonB"];
        flyUpAction = rightMap["FlyUp"];
        flyDownAction = rightMap["FlyDown"];

        // Left hand map
        var leftMap = playerInput.actions.FindActionMap("LeftHand", true);
        lookDirection = leftMap["Look"];
        rotateVisionAction = leftMap["RotateVision"];
        activatePower = leftMap["ActivatePower"];

        // Enable them explicitly
        rightMap.Enable();
        leftMap.Enable();
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