using Oculus.Interaction;
using UnityEngine;
using UnityEngine.InputSystem;

public class DesktopMovement : MonoBehaviour
{
    Vector2 horizontalInput;

    bool flyingUp;
    bool flyingDown;
    private float verticalSpeed;


    [SerializeField] private float maxVerticalVel;
    [SerializeField] private float verticalDecceleration;
    [SerializeField] private float verticalAcceleration;

    [SerializeField] private float horizontalAcceleration;
    [SerializeField] private float horizontalMaxVel;
    [SerializeField] private float horizontalDecceleration;

    private float currentHorizontalSpeed;
    private float targetHorizontalSpeed;

    private float speed;

    Vector3 linVel;

    Rigidbody rb;

    private void Start()
    {
        if (RigManager.instance.usingVr == true)
        {
            this.enabled = false;
            return;
        }

        rb = RigManager.instance.currentRb;

        Cursor.lockState = CursorLockMode.Locked;
    }

    private void FixedUpdate()
    {
        linVel = rb.linearVelocity;
        VerticalFlight();
        HorizontalControlls();
        rb.linearVelocity = linVel;
    }

    private void VerticalFlight()
    {
        float verticalInput = 0;
        if (flyingUp)
        {
            verticalInput += 1;
        }
        if (flyingDown)
        {
            verticalInput -= 1;
        }

        if (Mathf.Abs(verticalSpeed) < maxVerticalVel)
        {
            verticalSpeed += verticalAcceleration * Time.fixedDeltaTime * verticalInput;
        }
        else
        {
            verticalSpeed = maxVerticalVel * verticalInput;
        }

        if (verticalInput == 0)
        {
            //Stabilize vertical speed when close to 0.
            if (verticalSpeed > -0.5f && verticalSpeed < 0.5f)
            {
                verticalSpeed = 0;
            }

            if (verticalSpeed > 0)
            {
                verticalSpeed -= Time.fixedDeltaTime * verticalDecceleration;
            }
            else if (verticalSpeed < 0)
            {
                verticalSpeed += Time.fixedDeltaTime * verticalDecceleration;
            }
        }
    }

    private void HorizontalControlls()
    {
        if (horizontalInput == Vector2.zero)
        {
            targetHorizontalSpeed = 0;
        }
        else
        {
            targetHorizontalSpeed = horizontalMaxVel;
        }

        currentHorizontalSpeed = rb.linearVelocity.magnitude;

        float inputMagnitude = horizontalInput.magnitude;

        Vector3 inputDirection = RigManager.instance.pTransform.right * horizontalInput.x
            + RigManager.instance.pTransform.forward * horizontalInput.y;

        if (currentHorizontalSpeed < horizontalMaxVel || currentHorizontalSpeed > horizontalMaxVel)
        {
            speed = Mathf.Lerp(currentHorizontalSpeed, targetHorizontalSpeed * inputMagnitude,
                Time.fixedDeltaTime * horizontalAcceleration);

            speed = Mathf.Round(speed * 1000f) / 1000f;
        }
        else
        {
            speed = targetHorizontalSpeed;
        }

        Vector3 horizontalVel = inputDirection.normalized * (speed * Time.fixedDeltaTime);
        linVel = new Vector3(horizontalVel.x, verticalSpeed, horizontalVel.z);
    }

    public void FlyingInput(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        horizontalInput = input;
    }

    public void FlyUp(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            flyingUp = true;
        }
        else
        {
            flyingUp = false;
        }
    }

    public void FlyDown(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            flyingDown = true;
        }
        else
        {
            flyingDown = false;
        }
    }
}
