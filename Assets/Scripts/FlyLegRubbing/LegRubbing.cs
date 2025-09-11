using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class LegRubbing : MonoBehaviour
{
    public static LegRubbing Instance { get; private set; }

    [SerializeField] Transform leftHand;
    [SerializeField] Transform rightHand;
    [SerializeField] Transform center;
    [SerializeField] float maxDistance;
    [SerializeField] float minRubbingPerSecond = 0.1f;
    [SerializeField] float desktopInterval = 0.2f;

    public float TotalRubbing { get; private set; }
    public Action<float> chargeChange;

    InputControl lastRubControl;
    float lastRubTime;
    float lastDifference;
    bool inVR;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
    }

    private void Start()
    {
        inVR = RigManager.instance.usingVr;

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

    private void Update()
    {
        if (inVR) HandleVRRubbing();
    }

    private void HandleVRRubbing()
    {
        center.rotation = Quaternion.Slerp(Quaternion.identity, leftHand.rotation * rightHand.rotation, 0.5f);
        center.position = leftHand.position - (leftHand.position - rightHand.position) / 2;
        Matrix4x4 centerMatrix = center.worldToLocalMatrix;
        Vector3 leftPoint = centerMatrix * leftHand.position;
        Vector3 rightPoint = centerMatrix * rightHand.position;
        float distance = (new Vector3(leftPoint.x, 0, leftPoint.z) - new Vector3(rightPoint.x, 0, rightPoint.z)).sqrMagnitude;
        float difference = Mathf.Abs(leftPoint.y - rightPoint.y);
        float differenceInDifference = Mathf.Abs(difference - lastDifference);
        lastDifference = difference;
        if (Mathf.Pow(maxDistance, 2) > distance && differenceInDifference > minRubbingPerSecond * Time.deltaTime)
        {
            TotalRubbing += differenceInDifference;
            chargeChange?.Invoke(TotalRubbing);
        }
    }

    public void HandleDesktopRubbing(InputAction.CallbackContext callbackContext)
    {
        if (lastRubControl != callbackContext.control && Time.time - lastRubTime < desktopInterval)
        {
            TotalRubbing++;
            chargeChange(TotalRubbing);
        }
        lastRubTime = Time.time;
        lastRubControl = callbackContext.control;
    }
}
