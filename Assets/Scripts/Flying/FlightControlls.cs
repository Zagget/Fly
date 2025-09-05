using UnityEngine;
using UnityEngine.InputSystem;

public class FlightControlls : MonoBehaviour
{
    private void Start()
    {
        InputManager.Instance.flyUpAction.performed += FlyUp;
        InputManager.Instance.flyUpAction.canceled += FlyUp;
    }


    void FlyUp(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("Up");
        }
        else
        {
            Debug.Log("Stop");
        }
    }
}
