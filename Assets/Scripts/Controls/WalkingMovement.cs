using UnityEngine;
using UnityEngine.InputSystem;

public class WalkingMovement : MonoBehaviour
{
    [SerializeField] private float acceleration = 50;
    [SerializeField] private float maxVel = 1000;

    private float currentSpeed;
    private float targetSpeed;
    private float speed;
    private float verticalSpeed;

    Vector2 input;

    Vector3 linVel;
    Rigidbody rb;

    void Start()
    {
        rb = RigManager.instance.currentRb;
        if (rb == null) Debug.LogError("Rigidbody not found from RigManager!");
    }

    void FixedUpdate()
    {
        linVel = rb.linearVelocity;
        HorizontalControlls();
        rb.linearVelocity = linVel;
    }

    private void HorizontalControlls()
    {
        if (input == Vector2.zero)
        {
            targetSpeed = 0;
        }
        else
        {
            targetSpeed = maxVel;
        }

        currentSpeed = rb.linearVelocity.magnitude;

        float inputMagnitude = input.magnitude;

        Vector3 inputDirection = RigManager.instance.pTransform.right * input.x
            + RigManager.instance.pTransform.forward * input.y;

        if (currentSpeed < maxVel || currentSpeed > maxVel)
        {
            speed = Mathf.Lerp(currentSpeed, targetSpeed * inputMagnitude,
                Time.fixedDeltaTime * acceleration);

            speed = Mathf.Round(speed * 1000f) / 1000f;
        }
        else
        {
            speed = targetSpeed;
        }

        Vector3 horizontalVel = inputDirection.normalized * (speed * Time.fixedDeltaTime);
        linVel = new Vector3(horizontalVel.x, verticalSpeed, horizontalVel.z);
    }

    public void WalkingInput(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        this.input = input;
    }
}
