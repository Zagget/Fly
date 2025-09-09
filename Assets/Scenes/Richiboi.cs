using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Richiboi : MonoBehaviour
{
    public InputNewVR inputActions;

    private void Awake()
    {
        inputActions = new InputNewVR();
    }

    private void OnEnable()
    {
        // Enable the whole action map
        inputActions.ActionMap.Enable();

        // Subscribe to the button press
        inputActions.ActionMap.RightShoot.canceled += RightTrigger;
        inputActions.ActionMap.RightShoot.performed += RightTrigger;
        inputActions.ActionMap.RightShoot.started += RightTrigger;

        // Subscribe to the button press
        inputActions.ActionMap.LeftShoot.canceled += LeftTrigger;
        inputActions.ActionMap.LeftShoot.performed += LeftTrigger;
        inputActions.ActionMap.LeftShoot.started += LeftTrigger;
    }

    private void OnDisable()
    {
        // Subscribe to the button press
        inputActions.ActionMap.RightShoot.canceled -= RightTrigger;
        inputActions.ActionMap.RightShoot.performed -= RightTrigger;
        inputActions.ActionMap.RightShoot.started -= RightTrigger;

        // Subscribe to the button press
        inputActions.ActionMap.LeftShoot.performed -= LeftTrigger;
        inputActions.ActionMap.LeftShoot.canceled -= LeftTrigger;
        inputActions.ActionMap.LeftShoot.started -= LeftTrigger;

        // Enable the whole action map
        inputActions.ActionMap.Disable();
    }

    public void RightTrigger(InputAction.CallbackContext context)
    {
        Debug.Log("Right Trigger: " + context);

    }

    public void LeftTrigger(InputAction.CallbackContext context)
    {
        Debug.Log("Left Trigger: " + context);
    }
}
