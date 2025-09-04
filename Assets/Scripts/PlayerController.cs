using UnityEngine;
using UnityEngine.InputSystem;

public enum MovementState
{
    Flying,
    Walking,
    None // Used for menu and testing
}

public class PlayerController : MonoBehaviour
{
    [SerializeField] float speed;
    MovementState currentMov = MovementState.Flying;
    Rigidbody rb;

    private void Start()
    {
        rb = RigManager.instance.rb;
        if (rb == null) Debug.LogError("Rigidbody not found from RigManager!");

        SubscribeToInput();
    }

    private void SubscribeToInput()
    {
        InputManager.Instance.buttonAContext += OnAPressed;
        InputManager.Instance.r_JoyStickContext += OnMove;
        //InputManager.Instance.lookContext += OnLook;
    }

    private void OnDisable()
    {
        InputManager.Instance.buttonAContext -= OnAPressed;
        InputManager.Instance.r_JoyStickContext -= OnMove;
    }

    private void OnAPressed(InputAction.CallbackContext context)
    {
        if (context.performed) // Only trigger on button press
        {
            Debug.Log("Button A Pressed! Activating dash.");

        }
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        if (rb == null) return;

        Vector2 moveInput = context.ReadValue<Vector2>();

        Debug.Log($"MoveInput {moveInput}");

        switch (currentMov)
        {
            case MovementState.Flying:
                Flying(rb, moveInput);
                break;

            case MovementState.Walking:
                Walking(rb, moveInput);
                break;
        }
    }

    private void Flying(Rigidbody rb, Vector2 moveInput)
    {
        if (moveInput.sqrMagnitude < 0.01f) // deadzone
        {
            rb.linearVelocity = Vector3.zero;
            return;
        }

        // Use the player’s forward/right (ignoring camera rotation if camera is a child)
        Vector3 forward = transform.forward;
        Vector3 right = transform.right;

        // Flatten to avoid tilting movement if player rotates vertically
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        // Translate joystick into 3D direction
        Vector3 move = forward * moveInput.y + right * moveInput.x;

        rb.linearVelocity = move * speed;
    }


    private void Walking(Rigidbody rb, Vector2 moveInput)
    {

    }

}