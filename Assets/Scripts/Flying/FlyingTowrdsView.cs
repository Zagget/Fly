using UnityEngine;
using UnityEngine.InputSystem;

public class FlyingTowrdsView : MonoBehaviour
{
    [SerializeField] Transform character;
    [SerializeField] Rigidbody rb;
    [SerializeField] float speed;
    [SerializeField] float deceleration;
    [SerializeField] PlayerInput playerInput;
    [SerializeField] InputAction inputAction;

    bool isFlying;
    Vector3 velocity;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerInput.actions.FindAction("Jump").performed += toggleFlying;
    }

    private void FixedUpdate()
    {
        if (isFlying) velocity += character.forward * speed;
        velocity *= deceleration;

        rb.linearVelocity = velocity;
    }

    private void toggleFlying(InputAction.CallbackContext callbackContext)
    {
        inputAction.Enable();
        isFlying = !isFlying;
        Debug.Log(callbackContext.action);
    }


    private void OnDisable()
    {
        playerInput.actions.FindAction("Jump").performed -= toggleFlying;
    }
}
