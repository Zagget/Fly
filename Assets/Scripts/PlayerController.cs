using UnityEngine;

public enum MovementState
{
    Flying,
    Walking,
    None // Used for menu and testing
}

public class PlayerController : MonoBehaviour
{
    MovementState currentMov = MovementState.Flying;
    Rigidbody rb;


    private void Start()
    {
        rb = RigManager.instance.rb;
    }


    // Will prop change to event based movement.
    void Update()
    {
        switch (currentMov)
        {
            case MovementState.Flying:
                break;
            case MovementState.Walking:

                break;

            case MovementState.None:
                rb.linearVelocity = Vector3.zero;
                break;
        }
    }

    public void OnFlyInput()
    {
        currentMov = MovementState.Flying;
    }

    public void OnWalkingInput()
    {
        currentMov = MovementState.Walking;
    }

    public void FreezePlayer()
    {
        currentMov = MovementState.None;
    }
}