using UnityEngine;
using UnityEngine.InputSystem;
public class InputManager : MonoBehaviour
{
    private static InputManager _instance;
    public static InputManager Instance { get { return _instance; } }

    public Input inputActions;

    private void OnEnable()
    {
        inputActions.LeftHand.Enable();
        inputActions.RightHand.Enable();
    }

    private void OnDisable()
    {
        inputActions.LeftHand.Disable();
        inputActions.RightHand.Disable();
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

        inputActions = new Input();
    }
}