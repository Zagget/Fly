using UnityEngine;

public class LegRubbing : MonoBehaviour
{
    [SerializeField] Transform leftHand;
    [SerializeField] Transform rightHand;
    [SerializeField] Transform center;

    float lastDifference;
    float totalRubbing;

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
        totalRubbing += differenceInDifference;
        Debug.Log(totalRubbing);
    }

}
