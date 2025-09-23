using UnityEngine;
using UnityEngine.InputSystem;

public class WalkingMovement : MonoBehaviour
{
    [SerializeField] private float acceleration = 50;
    [SerializeField] private float maxVel = 1000;

    private float currentSpeed;
    private float targetSpeed;
    private float speed;

    Vector2 input;

    Vector3 linVel;
    Rigidbody rb;

    StateManager stateManager;
    RigManager rigManager;

    void Start()
    {
        stateManager = StateManager.Instance;
        rigManager = RigManager.instance;
        rb = rigManager.currentRb;
        if (rb == null) Debug.LogError("Rigidbody not found from RigManager!");

        stateManager.OnStateChanged += OnChangeState;
    }

    void FixedUpdate()
    {
        linVel = rb.linearVelocity;
        HorizontalControlls();
        rb.linearVelocity = linVel;
    }

    private void OnChangeState(BasePlayerState newState, BasePlayerState oldState)
    {
        if (newState == stateManager.walkingState)
        {
            this.enabled = true;
        }
        else if (oldState == stateManager.walkingState)
        {
            this.enabled = false;
            rb.linearVelocity = Vector3.zero;
            linVel = Vector3.zero;
        }
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

        Vector3 inputDirection = rigManager.pTransform.right * input.x
            + rigManager.pTransform.forward * input.y;

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
        linVel = new Vector3(horizontalVel.x, rb.linearVelocity.y, horizontalVel.z);
    }

    public void WalkingInput(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        this.input = input;
    }
}