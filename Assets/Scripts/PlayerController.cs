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
    [Header("Settings")]
    [SerializeField] float speed;
    [SerializeField] float rotateFactor;

    [Header("Ref")]
    [SerializeField] private GameObject vrRig;

    private MovementState currentMov = MovementState.Flying;

    // rb for pc or vr
    private Rigidbody rb;
    private Camera desktopCamera;

    private bool vr;

    private void Start()
    {
        rb = RigManager.instance.currentRb;
        if (rb == null) Debug.LogError("Rigidbody not found from RigManager!");

        vr = RigManager.instance.usingVr;

        if (!vr)
        {
            desktopCamera = RigManager.instance.desktopCamera;
            if (desktopCamera == null) Debug.LogError("Rigidbody not found from RigManager");
        }

        SubscribeToInputs();
    }

    private void SubscribeToInputs()
    {
        // Movement 
        InputManager.Instance.r_JoyStickAction.performed += OnMove;
        InputManager.Instance.rotateVisionAction.performed += OnRotateVision;
        if (!vr)
        {
            InputManager.Instance.lookDirection.performed += OnLook;
        }

        // Buttons
        InputManager.Instance.r_ButtonAAction.performed += OnAPressed;
        InputManager.Instance.r_ButtonBAction.performed += OnBPressed;

    }

    private void OnDisable()
    {
        InputManager.Instance.r_ButtonAAction.performed -= OnAPressed;
        InputManager.Instance.r_JoyStickAction.performed -= OnMove;
    }

    private void OnLook(InputAction.CallbackContext context)
    {
        Vector2 lookDelta = context.ReadValue<Vector2>();

        float lookSensitivity = 0.2f;
        float rotationX = lookDelta.y * lookSensitivity;
        float rotationY = lookDelta.x * lookSensitivity;

        desktopCamera.transform.Rotate(Vector3.left * rotationX);
        desktopCamera.transform.Rotate(Vector3.up * rotationY, Space.World);
    }

    private void OnAPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("Button A Pressed! Activating dash.");
        }
    }

    private void OnBPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("Button B Pressed! Activating dash.");
        }
    }

    private void OnRotateVision(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Vector2 rotateInput = context.ReadValue<Vector2>();
            Debug.Log($"RotateInput: {rotateInput}");

            if (vr)
            {
                Vector3 currentEuler = vrRig.transform.eulerAngles;
                currentEuler.y += rotateInput.x * rotateFactor; // rotate around Y only
                vrRig.transform.eulerAngles = currentEuler;
            }
            else
            {
                Vector3 currentEuler = desktopCamera.transform.eulerAngles;
                currentEuler.y += rotateInput.x * 10; // rotate around Y only
                desktopCamera.transform.eulerAngles = currentEuler;
            }
        }
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        if (rb == null) return;

        Vector2 moveInput = context.ReadValue<Vector2>();

        switch (currentMov)
        {
            case MovementState.Flying:

                // VR
                // FlyingScript(rb,moveinput, gameobject vrig or Vector3 currentEuler)
                // 
                // Pc
                // FlyingScript(rb, moveinput, camera)

                // Just for testing
                Flying(rb, moveInput);

                break;

            case MovementState.Walking:

                break;
        }
    }

    // Will be replaced by another script.
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
}