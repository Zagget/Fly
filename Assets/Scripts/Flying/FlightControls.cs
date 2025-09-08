using UnityEngine;
using UnityEngine.InputSystem;
public class FlightControls : MonoBehaviour
{
    private Rigidbody rb;

    [SerializeField] private float maxYVelocity;
    [SerializeField] private float minYVelocity;
    [SerializeField] private float yAcceleration;
    [SerializeField] private float flyDownSpeed;
    [SerializeField] private float yDecceleration;

    [SerializeField] private float horizontalAcceleration;
    [SerializeField] private float horizontalMaxVel;
    [SerializeField] private float horizontalDecceleration;

    private bool flyingUp = false;
    private bool flyingDown = false;

    private float minYVelThreashold = -10;
    private float maxYVelThreashold = 10;

    private float currentHorizontalSpeed;
    private float targetHorizontalSpeed;

    private float speed;
    private float accelerationTime;

    [SerializeField] private float verticalAcceleration;

    private Vector2 horizontalInput;

    float verticalSpeed;

    Vector3 linVel;

    private void Start()
    {
        rb = RigManager.instance.currentRb;
        if (rb == null) Debug.LogError("Rigidbody not found from RigManager!");


    }


    private void FixedUpdate()
    {
        linVel = rb.linearVelocity;

        FlyUpControlls();
        FlyDownControlls();
        HorizontalControlls();
        rb.linearVelocity = linVel;
    }

    private void FlyUpControlls()
    {
        if (verticalSpeed <= 0)
        {
            accelerationTime = 0;
        }
        else
        {
            accelerationTime += Time.fixedDeltaTime;
        }

        if (flyingUp)
        {
            if (verticalSpeed < maxYVelocity)
            {
                verticalSpeed += verticalAcceleration * Time.fixedDeltaTime;
            }
        }

        if (flyingDown)
        {
            if (verticalSpeed > minYVelocity)
            {
                verticalSpeed -= verticalAcceleration * Time.fixedDeltaTime;
            }
        }


        //if (!flyingUp) //if you are not trying to fly stabilize overtime.
        //{
        //    if (linVel.y > 0)
        //    {
        //        linVel = new Vector3(linVel.x,
        //            linVel.y - yDecceleration * Time.fixedDeltaTime, linVel.z);
        //    }
        //    else if (linVel.y > minYVelThreashold && linVel.y < maxYVelThreashold && !flyingDown)
        //    {
        //        linVel = new Vector3(linVel.x, 0, linVel.z);
        //    }
        //    return;
        //}

        //if (linVel.y < maxYVelocity * Time.fixedDeltaTime)
        //{
        //    linVel = new Vector3(linVel.x,
        //        linVel.y + yAcceleration * Time.fixedDeltaTime, linVel.z);
        //}
        //else
        //{
        //    linVel = new Vector3(linVel.x, maxYVelocity * Time.fixedDeltaTime, linVel.z);
        //}
    }

    private void FlyDownControlls()
    {
        if (flyingDown == false)
        {
            return;
        }

        linVel = new Vector3(linVel.x, -flyDownSpeed * Time.fixedDeltaTime, linVel.z);
    }

    private void HorizontalControlls()
    {
        if (horizontalInput == Vector2.zero)
        {
            targetHorizontalSpeed = 0;
        }

        targetHorizontalSpeed = horizontalMaxVel;

        currentHorizontalSpeed = rb.linearVelocity.magnitude;

        float inputMagnitude = horizontalInput.magnitude;

        Vector3 inputDirection = transform.right * horizontalInput.x + transform.forward * horizontalInput.y;

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

    public void FlyUp(bool performed)
    {
        if (performed)
        {
            flyingUp = true;
        }
        else
        {
            flyingUp = false;
        }
    }

    public void FlyDown(bool performed)
    {
        if (performed)
        {
            flyingDown = true;
        }
        else
        {
            flyingDown = false;
        }
    }

    public void FlyingInput(Vector2 input)
    {
        horizontalInput = input;
    }
}