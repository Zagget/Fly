using UnityEngine;

public class Hover : MonoBehaviour
{
    Rigidbody rb;

    float groundRayLength;

    float lowSpeedLimit;

    private void Start()
    {
        rb = RigManager.instance.currentRb;
    }

    private void Update()
    {
        Physics.Raycast(transform.position,-transform.up,groundRayLength);
    }

    private bool CheckIfLowSpeed()
    {
        if (rb.linearVelocity.sqrMagnitude < lowSpeedLimit * lowSpeedLimit)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
