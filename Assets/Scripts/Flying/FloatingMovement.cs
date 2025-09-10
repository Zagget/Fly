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

        float xValue = 0;
        float yValue = 0;
        float zValue = 0;
        if (usingVr)
        {
            leftController = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LHand);
            rightController = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RHand);

            rightController.y -= 1; // this makes the controller be tracked more accurately as 0 was floor before.
            leftController.y -= 1;

            xValue += leftController.x * 100;
            xValue += rightController.x * 100;
            xValue = xValue / 2;

            yValue = ((leftController.y + rightController.y) / 2) * 100;
            //yValue -= tempYOffset;

            zValue += leftController.z * 100;
            zValue += rightController.z * 100;
            zValue = (zValue / 2) - tempZOffset;

           // if (zValue < 0) zValue = 0;

            if (Mathf.Abs(xValue) < deadZone) xValue = 0;

            if (Mathf.Abs(yValue) < deadZone) yValue = 0;

            if (Mathf.Abs(zValue) < deadZone) zValue = 0;
        }

        Vector3 inputDirection = new Vector3(xValue, yValue, zValue).normalized;

        linVel = Vector3.zero;

        linVel += RigManager.instance.currentRb.transform.forward * Time.fixedDeltaTime * fixedSpeed * zValue;
        linVel += RigManager.instance.currentRb.transform.up * Time.fixedDeltaTime * fixedSpeed * yValue;
        linVel += RigManager.instance.currentRb.transform.right * Time.fixedDeltaTime * fixedSpeed * xValue;

        if (leftController.z <= 0.1f && rightController.z > 0.3f)
        {
            linVel += RigManager.instance.currentRb.transform.forward * Time.fixedDeltaTime * fixedSpeed * zValue * 10;
        }

        Debug.Log(" X: " + xValue + " " + " Y: " + yValue + " " + " Z: " + zValue);

        //if (Vector3.Dot(linVel, activeCamera.transform.forward) < 0)
        //{
        //    linVel += activeCamera.transform.forward * Time.fixedDeltaTime * decceleration;

        //}
    }

    public void FlyingInput(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
    }
}