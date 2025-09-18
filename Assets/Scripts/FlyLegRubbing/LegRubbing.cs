using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class LegRubbing : MonoBehaviour
{
    public static LegRubbing Instance { get; private set; }

    [SerializeField] Transform leftHand;
    [SerializeField] Transform rightHand;
    [SerializeField] Transform center;
    [SerializeField] Transform handRotation;
    [SerializeField, Tooltip("The distance the two controllers can be away from each other in the plane with the normal of the ideal rubbing direction")] float maxDistance;
    [SerializeField] float minRubbingPerSecond = 0.1f;
    [SerializeField, Tooltip("The amount of time allowed in between rubbing to be considered one rubbing")] float rubInterval = 0.2f;
    [SerializeField, Tooltip("The amount of time required to start gaining charge")] float timeToGainCharge = 0.5f;

    public float TotalRubbing { get; private set; }
    public Action<float> chargeChange;

    Matrix4x4 rotationForMaxRubbing;
    InputControl lastRubControl;
    float lastRubTime;
    float lastDifference;
    float timeSpentRubbing;
    bool inVR;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
    }

    private void Start()
    {
        inVR = RigManager.instance.usingVr;
        rotationForMaxRubbing = handRotation.worldToLocalMatrix;

        if (inVR && (leftHand == null || rightHand == null || center == null))
        {
            gameObject.SetActive(false);
            return;
        }
    }

    public float ResetRubbing()
    {
        float temp = TotalRubbing;
        TotalRubbing = 0;
        chargeChange?.Invoke(TotalRubbing);
        return temp;
    }

    public float RemoveRubbing(float amount)
    {
        TotalRubbing -= amount;
        if (TotalRubbing < 0) { TotalRubbing = 0; }
        chargeChange?.Invoke(TotalRubbing);
        return TotalRubbing;
    }

    private void Update()
    {
        if (inVR) HandleVRRubbing();
    }

    private void HandleVRRubbing()
    {
        center.rotation = Quaternion.Slerp(Quaternion.identity, leftHand.rotation * rightHand.rotation, 0.5f);
        center.position = leftHand.position - (leftHand.position - rightHand.position) / 2;
        Matrix4x4 centerMatrix = center.worldToLocalMatrix * rotationForMaxRubbing;
        Vector3 leftPoint = centerMatrix * leftHand.position;
        Vector3 rightPoint = centerMatrix * rightHand.position;
        float distance = (new Vector3(leftPoint.x, 0, leftPoint.z) - new Vector3(rightPoint.x, 0, rightPoint.z)).sqrMagnitude;
        float difference = Mathf.Abs(leftPoint.y - rightPoint.y);
        float differenceInDifference = Mathf.Abs(difference - lastDifference);
        lastDifference = difference;

        if (Mathf.Pow(maxDistance, 2) > distance && differenceInDifference > minRubbingPerSecond * Time.deltaTime)
        {
            lastRubTime = Time.time;
        }

        if (Time.time - lastRubTime < rubInterval)
        {
            timeSpentRubbing += Time.deltaTime;
            chargeChange?.Invoke(TotalRubbing);
        }
        else timeSpentRubbing = 0;

        if (timeSpentRubbing >= timeToGainCharge) TotalRubbing += Time.deltaTime;

    }

    public void HandleDesktopRubbing(InputAction.CallbackContext callbackContext)
    {
        if (lastRubControl != callbackContext.control)
        {
            TotalRubbing++;
            chargeChange?.Invoke(TotalRubbing);
        }
        lastRubControl = callbackContext.control;
    }
}
