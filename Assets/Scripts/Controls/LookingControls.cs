using System.Collections;
using UnityEngine;

public enum RotationStep
{
    Deg180 = 179,
    Deg90 = 90,
    Deg45 = 45
}

public class LookingControls : MonoBehaviour
{
    [Header("Mouse Settings")]
    float lookSensitivity = 0.2f;

    [Header("Rotation Settings")]
    [SerializeField] private RotationStep rotationStep = RotationStep.Deg90;
    [Range(0, 0.5f)][SerializeField] float rotateSmothness = 0.5f;

    private float rotationDegree => (float)rotationStep;
    private bool isRotating = false;

    private Transform pTransform;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pTransform = RigManager.instance.pTransform;
    }

    public void OnLook(Vector2 mouseInput)
    {
        if (isRotating) return;

        float rotationX = mouseInput.y * lookSensitivity;
        float rotationY = mouseInput.x * lookSensitivity;

        pTransform.transform.Rotate(Vector3.left * rotationX);
        pTransform.transform.Rotate(Vector3.up * rotationY, Space.World);
    }

    public IEnumerator RotateVision(Vector2 rotateInput)
    {
        if (isRotating) yield break;
        isRotating = true;

        Vector3 currentEuler = Vector3.zero;

        Vector3 forward = pTransform.forward;
        forward.y = 0f;
        forward.Normalize();
        float referenceYaw = Quaternion.LookRotation(forward).eulerAngles.y;

        float targetY = referenceYaw + rotateInput.x * rotationDegree;

        float smoothVelocity = 0f;
        float smoothTime = Mathf.Max(0.001f, rotateSmothness);

        while (true)
        {
            float currentY = pTransform.eulerAngles.y;

            float newY = Mathf.SmoothDampAngle(currentY, targetY, ref smoothVelocity, smoothTime);

            Vector3 e = pTransform.eulerAngles;
            e.y = newY;
            pTransform.eulerAngles = e;

            if (Mathf.Abs(Mathf.DeltaAngle(newY, targetY)) < 0.1f)
                break;

            yield return null;
        }
        isRotating = false;
    }
}