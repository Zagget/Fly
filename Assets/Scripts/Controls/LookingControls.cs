using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public enum RotationStep
{
    Deg180 = 179,
    Deg90 = 90,
    Deg45 = 45,
    Deg30 = 30,
    Deg20 = 20,
    Deg1 = 1
}

public class LookingControls : MonoBehaviour
{
    [Header("Mouse Settings")]
    float lookSensitivity = 0.2f;

    [Header("Rotation Settings")]
    [SerializeField] private RotationStep rotationStep = RotationStep.Deg90;
    [Range(0, 0.5f)][SerializeField] private float rotateSmothness = 0.5f;

    private float rotationDegree => (float)rotationStep;

    [SerializeField] private float maxCooldown = 0.4f;
    [SerializeField] private float minCooldown = 0.02f;

    private bool holdingDown;
    private bool isRotating = false;

    private Coroutine rotateCor;

    private Transform pTransform;
    private bool vr;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pTransform = RigManager.instance.pTransform;
        vr = RigManager.instance.usingVr;
    }

    public void OnLook(Vector2 mouseInput)
    {
        if (isRotating) return;

        float rotationX = mouseInput.y * lookSensitivity;
        float rotationY = mouseInput.x * lookSensitivity;

        pTransform.transform.Rotate(Vector3.left * rotationX);
        pTransform.transform.Rotate(Vector3.up * rotationY, Space.World);
    }

    public void OnRotate(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            holdingDown = true;
            Vector2 input = context.ReadValue<Vector2>();
            StartCoroutine(RotateWhileHolding(input));
        }

        if (context.canceled)
        {
            holdingDown = false;
        }
    }

    void Update()
    {
        Debug.Log($"blä holdingDown: {holdingDown}");
    }

    private IEnumerator RotateWhileHolding(Vector2 input)
    {
        while (holdingDown)
        {
            if (!isRotating)
            {
                if (rotateCor != null)
                    StopCoroutine(rotateCor);

                rotateCor = StartCoroutine(RotateVision(input));
            }
            yield return null;
        }
        yield break;
    }

    private IEnumerator RotateVision(Vector2 rotateInput)
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

        float angleDelta = Mathf.Abs(rotationDegree);
        float t = angleDelta / 180f;
        float cooldown = Mathf.Lerp(minCooldown, maxCooldown, t);

        Debug.Log($"blä Cooldown {cooldown}");
        yield return new WaitForSeconds(cooldown);

        // Debug.Log($"blä smoothtime: {smoothTime}");
        // Debug.Log($"blä cooldown:  {cooldown}");
        isRotating = false;
    }
}