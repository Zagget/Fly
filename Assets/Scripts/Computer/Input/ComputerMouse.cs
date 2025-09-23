using UnityEngine;

public class ComputerMouse : MonoBehaviour
{
    [SerializeField] Transform laserOrigin;
    [SerializeField] ComputerInputManager inputManager;

    [SerializeField] float laserLength;

    Vector3 lastPoint;
    bool hitLastTime = false;

    private void FixedUpdate()
    {
        if (Physics.Raycast(laserOrigin.transform.position, laserOrigin.transform.up, out RaycastHit hitInfo, laserLength))
        {
            if (hitLastTime && inputManager != null) inputManager.MoveMouse(hitInfo.point - lastPoint);
            lastPoint = hitInfo.point;
            hitLastTime = true;
        }
        else hitLastTime = false;
    }
}
