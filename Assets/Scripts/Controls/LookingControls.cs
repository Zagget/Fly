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

    private bool holdingDown;
    private bool isRotating = false;

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

    private IEnumerator RotateWhileHolding(Vector2 input)
    {
        while (holdingDown)
        {
            if (!isRotating)
            {
                yield return RotateVision(input);
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

        float cooldown;

        if (rotationStep == RotationStep.Deg1)
            cooldown = 0.02f;

        else
            cooldown = Mathf.Clamp(0.2f - smoothTime, 0f, 0.2f);

        yield return new WaitForSeconds(cooldown);

        // Debug.Log($"blä smoothtime: {smoothTime}");
        // Debug.Log($"blä cooldown:  {cooldown}");
        isRotating = false;
    }
}