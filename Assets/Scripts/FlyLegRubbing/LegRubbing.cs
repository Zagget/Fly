using UnityEngine;

public class LegRubbing : MonoBehaviour
{
    [SerializeField] Transform leftHand;
    [SerializeField] Transform rightHand;
    [SerializeField] Transform center;
    [SerializeField] float maxDistance;
    [SerializeField] float minRubbingPerSecound = 0.1f;
    
    public float TotalRubbing { get; private set; }

    float lastDifference;

    private void Start()
    {
        if (leftHand == null || rightHand == null || center == null) gameObject.SetActive(false);
    }

    public void ResetRubbing() { TotalRubbing = 0; }

    private void Update()
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
        if (Mathf.Pow(maxDistance, 2) > distance && differenceInDifference > minRubbingPerSecound * Time.deltaTime) TotalRubbing += differenceInDifference;
    }
}
