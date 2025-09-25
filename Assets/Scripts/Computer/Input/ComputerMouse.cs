using UnityEngine;

public class ComputerMouse : MonoBehaviour
{
    [SerializeField] Transform laserOrigin;
    [SerializeField] ComputerInputManager inputManager;

    [SerializeField] float laserLength;

    [SerializeField] Vector3 temp;

    Vector3 lastPoint;
    bool hitLastTime = false;

    private void FixedUpdate()
    {
        if (Physics.Raycast(laserOrigin.transform.position, laserOrigin.transform.up, out RaycastHit hitInfo, laserLength))
        {
            Vector3 relativePoint = transform.InverseTransformDirection(hitInfo.point);
            if (hitLastTime && inputManager != null) inputManager.MoveMouse(relativePoint - lastPoint);
            lastPoint = relativePoint;
            hitLastTime = true;
        }
        else hitLastTime = false;
    }
}
