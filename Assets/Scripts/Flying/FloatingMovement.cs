using UnityEngine;
using UnityEngine.InputSystem;

public class FloatingMovement : MonoBehaviour
{
    private Rigidbody rb;

    [SerializeField] private float maxVerticalVel;
    [SerializeField] private float verticalDecceleration;
    [SerializeField] private float verticalAcceleration;

    [SerializeField] private float acceleration;
    [SerializeField] private float fixedSpeed;
    [SerializeField] private float decceleration;

    [SerializeField] private float tempYOffset;
    [SerializeField] private float tempZOffset;

    [SerializeField] private float deadZone;

    [SerializeField] Camera vrCam;

    private Vector3 leftController;
    private Vector3 rightController;
    private Vector3 controllerAverage;

    private bool flyingUp = false;
    private bool flyingDown = false;

    float speed;

    private Vector2 horizontalInput;

    private float verticalSpeed;

    Vector3 linVel; //rb.linearVelocity

    Camera activeCamera = null;

    Vector2 horizontalVelocity;
    [SerializeField] private float maxSpeed = 50;

    bool usingVr = false;

    private void Start()
    {
        rb = RigManager.instance.currentRb;
        if (rb == null) Debug.LogError("Rigidbody not found from RigManager!");

        if (RigManager.instance.usingVr == true)
        {
            usingVr = true;
            activeCamera = vrCam;
        }
        else
        {
            activeCamera = RigManager.instance.desktopCamera;
        }
    }


    private void FixedUpdate()
    {
        linVel = rb.linearVelocity;

        //VerticalFlight();
        ForwardFlight();
        rb.linearVelocity = linVel;
    }

    private void ForwardFlight()
    {
        //Vector3 inputDirection = activeCamera.transform.right * horizontalInput.x
        //    + activeCamera.transform.forward * horizontalInput.y;

        float xValue = 0;
        float yValue = 0;
        float zValue = 0;
        if (usingVr)
        {
            leftController = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LHand);
            rightController = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RHand);

            xValue += leftController.x * 100;
            xValue += rightController.x * 100;
            xValue = xValue / 2;

            yValue = ((leftController.y + rightController.y) / 2) * 100;
          //  yValue -= tempYOffset;

            zValue += leftController.z * 100;
            zValue += rightController.z * 100;
            zValue = (zValue / 2) - tempZOffset;

            if (zValue < 0) zValue = 0;

            if (xValue < deadZone) xValue = 0;

            if (yValue < deadZone) yValue = 0;

            if (zValue < deadZone) zValue = 0;
        }
        else
        {
           // XValue = activeCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue().normalized).x;
        }   

        float verticalInput = 0;
        float horiInput = 0;
        //if (inputDirection != Vector3.zero)
        //{
        //    verticalInput = horizontalInput.y;
        //    horiInput = horizontalInput.x;
        //    linVel.x = linVel.x / 1.5f;
        //}

        Vector3 inputDirection = new Vector3(xValue, yValue, zValue).normalized;

        linVel = Vector3.zero;

        linVel += RigManager.instance.currentRb.transform.forward * Time.fixedDeltaTime * fixedSpeed * zValue;
        linVel += RigManager.instance.currentRb.transform.up * Time.fixedDeltaTime * fixedSpeed * yValue;
        linVel += RigManager.instance.currentRb.transform.right * Time.fixedDeltaTime * fixedSpeed * xValue;

        Debug.Log(" X: " + xValue + " " + " Y: " + yValue + " " + " Z: " + zValue);

        //if (linVel.sqrMagnitude < maxSpeed * maxSpeed)
        //{
            
        //}

        //if (Vector3.Dot(linVel, activeCamera.transform.forward) < 0)
        //{
        //    linVel += activeCamera.transform.forward * Time.fixedDeltaTime * decceleration;

        //}
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

    public void FlyingInput(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        horizontalInput = input;
    }
}