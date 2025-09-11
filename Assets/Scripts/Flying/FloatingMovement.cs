using UnityEngine;
using UnityEngine.InputSystem;

public class FloatingMovement : MonoBehaviour
{
    private Rigidbody rb;

    [SerializeField] private float fixedSpeed;
    [SerializeField] private float highSpeedMode;

    [SerializeField] private float controllerYOffset = 100; //Offset is aproxematly from ground, where ~0 is the floor.
    [SerializeField] private float controllerZOffset; //offset is how close controllers have to be to the player to count as 0 or negative.

    [SerializeField] private float deadZone;

    private Camera vrCam;

    private Vector3 leftController;
    private Vector3 rightController;

    private Vector3 linVel; //rb.linearVelocity

    private Camera activeCamera = null;

    [SerializeField] private float maxSpeed = 50;

    private float SMLeftMinimum = 0.1f;
    private float SMRightMinimum = 0.3f;

    Vector3 controllerPositionInput;

    private float turnStrength;
    private Vector2 rightAxisInput;

    private FlightMode currentMode;
    private enum FlightMode
    {
        Standard = 0,
        Fast = 1,
    }


    private void Start()
    {
        rb = RigManager.instance.currentRb;
        if (rb == null) Debug.LogError("Rigidbody not found from RigManager!");

        if (RigManager.instance.usingVr == true)
        {
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

        controllerPositionInput = GetControllerPositions();
        currentMode = GetFlightMode();
        switch (currentMode)
        {
            case FlightMode.Standard:
                StandardControls();
                break;

            case FlightMode.Fast:
                FastControls();
                break;
        }

        rb.linearVelocity = linVel;
    }

    private FlightMode GetFlightMode()
    {
        if (rightAxisInput != Vector2.zero) //Super man pose.
        {
            return FlightMode.Fast;
        }
        else
        {
            return FlightMode.Standard;
        }
    }

    private void StandardControls()
    {
        Vector3 inputDirection = controllerPositionInput.normalized;

        linVel = Vector3.zero;

        linVel += RigManager.instance.currentRb.transform.forward * Time.fixedDeltaTime * fixedSpeed * controllerPositionInput.z;
        linVel += RigManager.instance.currentRb.transform.up * Time.fixedDeltaTime * fixedSpeed * controllerPositionInput.y;
        linVel += RigManager.instance.currentRb.transform.right * Time.fixedDeltaTime * fixedSpeed * controllerPositionInput.x;
    }

    private Vector3 GetControllerPositions()
    {
        float xValue = 0;
        float yValue = 0;
        float zValue = 0;

        leftController = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LHand);
        rightController = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RHand);


        xValue = ((leftController.x + rightController.x) / 2) * 100;

        yValue = ((leftController.y + rightController.y) / 2) * 100;
        yValue -= controllerYOffset;

        zValue = ((leftController.z + rightController.z) / 2) * 100;
        zValue -= controllerZOffset;

        // if (zValue < 0) zValue = 0;

        if (Mathf.Abs(xValue) < deadZone) xValue = 0;

        if (Mathf.Abs(yValue) < deadZone) yValue = 0;

        if (Mathf.Abs(zValue) < deadZone) zValue = 0;


        Debug.Log(" X: " + xValue + " " + " Y: " + yValue + " " + " Z: " + zValue);

        return new Vector3(xValue, yValue, zValue);
    }

    private void FastControls()
    {
        Vector3 forward = RigManager.instance.currentRb.transform.forward;
        Vector3 right = RigManager.instance.currentRb.transform.right;

        

        Vector3 moveDirection = forward;
        moveDirection = Vector3.Lerp(moveDirection, rightAxisInput.normalized * forward, Time.fixedDeltaTime * turnStrength);
        linVel = moveDirection * highSpeedMode;
    }

    public void FlyingInput(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        rightAxisInput = input;
    }
}