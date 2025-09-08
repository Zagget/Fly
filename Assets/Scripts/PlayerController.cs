using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEditor.ShaderGraph.Internal;

public enum MovementState
{
    Flying,
    Walking,
    None // Used for menu and testing
}

public class PlayerController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float movSpeed;

    [Header("Rotation Settings")]
    [SerializeField] float pcRotationFactor = 10;
    [SerializeField] float vrRotationFactor = 10;
    [Range(0, 0.5f)][SerializeField] float rotateSmothness = 0.5f;
    [Range(0, 2)][SerializeField] float rotateCooldown = 1f;
    private float rotationVelo = 0f;

    [Header("Ref")]
    [SerializeField] private GameObject vrRig;
    [SerializeField] private FlightControls flightControls;

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
        InputManager.Instance.flyUpAction.performed += FlyUp;
        InputManager.Instance.flyUpAction.canceled += FlyUp;

        InputManager.Instance.flyDownAction.performed += FlyDown;
        InputManager.Instance.flyDownAction.canceled += FlyDown;
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

    private void FlyUp(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            flightControls.FlyUp(true);
        }
        else
        {
            flightControls.FlyUp(false);
        }
    }

    private void FlyDown(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            flightControls.FlyDown(true);
        }
        else
        {
            flightControls.FlyDown(false);
        }
    }

    private void OnLook(InputAction.CallbackContext context)
    {
        if (isRotating) return;
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

        }
    }

    private void OnBPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {

        }
    }


    private bool isRotating = false;
    private void OnRotateVision(InputAction.CallbackContext context)
    {
        // if (context.performed)
        // {
        Vector2 rotateInput = context.ReadValue<Vector2>();

        if (!isRotating)
        {
            StartCoroutine(RotateVision(rotateInput));
        }
        //}
    }

    private IEnumerator RotateVision(Vector2 rotateInput)
    {
        if (isRotating) yield break;
        isRotating = true;

        Debug.Log("Started Rotating");
        float targetY;

        if (vr)
        {
            targetY = vrRig.transform.eulerAngles.y + rotateInput.x * pcRotationFactor;
        }
        else
        {
            targetY = desktopCamera.transform.eulerAngles.y + rotateInput.x * vrRotationFactor;
        }

        Vector3 currentEuler = Vector3.zero;

        while (true)
        {
            if (vr)
            {
                currentEuler = vrRig.transform.eulerAngles;
                float smoothTime = Mathf.Lerp(0, 0.5f, rotateSmothness);
                float newY = Mathf.SmoothDampAngle(currentEuler.y, targetY, ref rotationVelo, smoothTime);
                currentEuler.y = newY;
                vrRig.transform.eulerAngles = currentEuler;

                if (GetIsAngleMatching(newY, targetY))
                    break;
            }
            else
            {
                currentEuler = desktopCamera.transform.eulerAngles;
                float smoothTime = Mathf.Lerp(0, 1, rotateSmothness);
                float newY = Mathf.SmoothDampAngle(currentEuler.y, targetY, ref rotationVelo, smoothTime);
                currentEuler.y = newY;
                desktopCamera.transform.eulerAngles = currentEuler;

                if (GetIsAngleMatching(newY, targetY))
                    break;
            }

            yield return null;
        }

        Debug.Log("Done rotating");
        isRotating = false;
    }

    private bool GetIsAngleMatching(float newY, float targetY)
    {
        return (Mathf.Abs(Mathf.DeltaAngle(newY, targetY)) < 0.1f);
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        if (rb == null) return;

        Vector2 moveInput = context.ReadValue<Vector2>();

        switch (currentMov)
        {
            case MovementState.Flying:
                flightControls.FlyingInput(moveInput);

                break;

            case MovementState.Walking:

                break;
        }
    }

    // Testing
    // private void Flying(Rigidbody rb, Vector2 moveInput)
    // {
    //     if (moveInput.sqrMagnitude < 0.01f) // deadzone
    //     {
    //         rb.linearVelocity = Vector3.zero;
    //         return;
    //     }

    //     // Use the player’s forward/right (ignoring camera rotation if camera is a child)
    //     Vector3 forward = transform.forward;
    //     Vector3 right = transform.right;

    //     // Flatten to avoid tilting movement if player rotates vertically
    //     forward.y = 0f;
    //     right.y = 0f;
    //     forward.Normalize();
    //     right.Normalize();

    //     // Translate joystick into 3D direction
    //     Vector3 move = forward * moveInput.y + right * moveInput.x;

    //     rb.linearVelocity = move * movSpeed;
    // }
}