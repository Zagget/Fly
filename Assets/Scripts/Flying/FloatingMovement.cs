using System.Collections;
using UnityEngine;
public class FloatingMovement : MonoBehaviour
{
    private Rigidbody rb;

    [SerializeField] private float fixedSpeed;
    [SerializeField] private float highSpeedMode;

    [SerializeField] private float controllerYOffset = 100; //Offset is approximately from ground, where ~0 is the floor.
    [SerializeField] private float controllerZOffset; //offset is how close controllers have to be to the player to count as 0 or negative.

    [SerializeField] private float deadZone;
    [SerializeField] private Transform centerEyeTransform;
    [SerializeField] private float headsetYOffset = 1.15f;

    private Vector3 leftController;
    private Vector3 rightController;

    private Vector3 linVel; //rb.linearVelocity

    [SerializeField] private float maxSpeed = 50;

    private Vector3 controllerPositionInput;
    private PlayerController controller;

    private float timeToSlowInput = 1;
    private float controllerInputMultiplier = 1;

    private void Start()
    {
        StateManager.Instance.OnStateChanged += OnSateChanged;

        if (RigManager.instance.usingVr == false)
        {
            this.enabled = false;
            return;
        }

        rb = RigManager.instance.currentRb;
        if (rb == null) Debug.LogError("Rigidbody not found from RigManager!");

        controller = GetComponent<PlayerController>();
        if (controller == null) Debug.LogError("Floating movement does not have access to player controller");
    }

    private void FixedUpdate()
    {
        linVel = rb.linearVelocity;

        controllerPositionInput = GetControllerPositions();
        StandardControls();

        rb.linearVelocity = linVel;
    }

    void OnSateChanged(BasePlayerState state, BasePlayerState lastState)
    {
        if (state == StateManager.Instance.flyingState)
        {
            this.enabled = true;
            if (lastState == StateManager.Instance.hoverState)
            {
                StartCoroutine(nameof(SlowControllerInput));
            }

        }
        else if (this.enabled == true)
        {
            rb.linearVelocity = Vector3.zero;
            linVel = rb.linearVelocity;
            this.enabled = false;
        }
    }

    private void StandardControls()
    {
        Vector3 inputDirection = controllerPositionInput.normalized;

        linVel = Vector3.zero;

        linVel += RigManager.instance.currentRb.transform.forward
            * Time.fixedDeltaTime * fixedSpeed * controllerPositionInput.z * controllerInputMultiplier;

        linVel += RigManager.instance.currentRb.transform.up
            * Time.fixedDeltaTime * fixedSpeed * controllerPositionInput.y * controllerInputMultiplier;

        linVel += RigManager.instance.currentRb.transform.right
            * Time.fixedDeltaTime * fixedSpeed * controllerPositionInput.x * controllerInputMultiplier;
    }

    IEnumerator SlowControllerInput()
    {
        float timer = 0;

        while (timer < timeToSlowInput) 
        {
            controllerInputMultiplier = timer / timeToSlowInput;
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        controllerInputMultiplier = 1;
    }

    private Vector3 GetControllerPositions()
    {
        float xValue = 0;
        float yValue = 0;
        float zValue = 0;

        leftController = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LHand);
        rightController = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RHand);


        Vector3 cPos = centerEyeTransform.localPosition;

        Vector3 offsetTracking = ((leftController + rightController) / 2);



        xValue = (((leftController + rightController).x / 2) - cPos.x) * 100;

        yValue = (((leftController + rightController).y / 2) - (cPos.y - headsetYOffset)) * 100;
        yValue -= controllerYOffset;

        zValue = (((leftController + rightController).z / 2) - cPos.z) * 100;
        zValue -= controllerZOffset;

        if (Mathf.Abs(xValue) < deadZone) xValue = 0;

        if (Mathf.Abs(yValue) < deadZone) yValue = 0;

        if (Mathf.Abs(zValue) < deadZone) zValue = 0;


        //Debug.Log(" X: " +  xValue + " " + " Y: " + yValue + " " + " Z: " + zValue);

        return new Vector3(xValue, yValue, zValue);
    }
}