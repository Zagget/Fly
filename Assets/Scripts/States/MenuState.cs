using UnityEngine;
using UnityEngine.InputSystem;

public class MenuState : BasePlayerState
{
    public override void HandleMovement(InputAction.CallbackContext context, FloatingMovement movement)
    {
        //Joystick can select in menu as well
    }
}