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

[System.Serializable]
public class RotationSetting
{
    [SerializeField] private RotationStep rotationStep = RotationStep.Deg30;
    [Range(0f, 0.5f)][SerializeField] private float rotateSmoothness = 0.25f;

    public void SetCustomValues(RotationStep step, float smoothness)
    {
        rotationStep = step;
        rotateSmoothness = Mathf.Clamp(smoothness, 0f, 0.5f);
    }
}

public class LookingControls : MonoBehaviour
{
    [Header("Mouse Settings")]
    float lookSensitivity = 0.2f;

    [Header("Rotation Settings")]
    [SerializeField] private RotationStep rotationStep = RotationStep.Deg90;
    [Range(0, 0.5f)][SerializeField] private float rotateSmothness = 0.5f;

    [SerializeField] private SettingsData settingsData;

    private float rotationDegree => (float)rotationStep;

    [SerializeField] private float maxCooldown = 0.4f;
    [SerializeField] private float minCooldown = 0.02f;

    private bool holdingDown;
    private bool isRotating = false;

    private Coroutine rotateCor;
    private Transform pTransform;

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

        float startY = pTransform.eulerAngles.y;
        float targetY = startY;
        // +Mathf.Sign(rotateInput.x) * rotationDegree;

        if (rotateInput.x > 0f)
            targetY += rotationDegree;
        else if (rotateInput.x < 0f)
            targetY -= rotationDegree;

        // Debug.Log($"blä startY {startY} targety {targetY}");

        if (rotateSmothness <= 0f)
        {
            // Snap instantly
            Vector3 snapEuler = pTransform.eulerAngles;
            snapEuler.y = targetY;
            pTransform.eulerAngles = snapEuler;
        }
        else
        {
            float duration = rotateSmothness;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / duration);

                float newY = Mathf.LerpAngle(startY, targetY, t);

                Vector3 e = pTransform.eulerAngles;
                e.y = newY;
                pTransform.eulerAngles = e;

                yield return null;
            }
        }

        // Cooldown based on angle
        float angleDelta = Mathf.Abs(rotationDegree);
        float tCooldown = angleDelta / 180f;
        float cooldown = Mathf.Lerp(minCooldown, maxCooldown, tCooldown);

        //Debug.Log($"Cooldown {cooldown}");
        yield return new WaitForSeconds(cooldown);

        isRotating = false;
    }
}