using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private static InputManager _instance;
    public static InputManager Instance {  get { return _instance; } }

    private InputAction r_JoyStickAction;
    private PlayerInput playerInput;

    private void OnEnable()
    {
        r_JoyStickAction = playerInput.actions["Move"];
        r_JoyStickAction.performed += DebugInputs;
    }

    private void OnDisable()
    {
        
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

    void DebugInputs(InputAction.CallbackContext context)
    {
        Debug.Log(context);
    }
}
