using UnityEngine;

public class Hover : MonoBehaviour
{
    Rigidbody rb;

    float groundRayLength;

    private void Start()
    {
        rb = RigManager.instance.currentRb;
    }

    private void Update()
    {
        Physics.Raycast(transform.position,-transform.up,groundRayLength);
    }


}
