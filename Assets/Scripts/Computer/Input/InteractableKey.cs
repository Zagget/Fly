using UnityEngine;

public abstract class InteractableKey : MonoBehaviour
{
    [Header("Physical key")]
    [SerializeField] float distance;
    [SerializeField] float pushback;
    [SerializeField] float tolerance;
    [SerializeField] float deceleration = 0.4f;

    [Header("References")]
    [SerializeField] protected ComputerInputManager inputManager;
    [SerializeField] Rigidbody parent;
    Rigidbody rb;

    Vector3 startPos;
    bool isPressed = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        startPos = transform.localPosition;
        if (parent == null) Destroy(this);
    }

    private void Update()
    {
        rb.rotation = parent.rotation;
        Vector3 newPos = startPos;
        newPos.y = Mathf.Clamp(transform.localPosition.y, startPos.y - distance, startPos.y);
        rb.position = parent.transform.TransformPoint(newPos);

        if (transform.localPosition.y < startPos.y - tolerance) 
        {
            if(!isPressed) OnPress(); 
            isPressed = true;
        }
        else isPressed = false;
    }

    private void FixedUpdate()
    {
        rb.angularVelocity = Vector3.zero;

        rb.linearVelocity += transform.parent.TransformVector(Vector3.up) * pushback;
        rb.linearVelocity *= deceleration;
    }

    protected abstract void OnPress();
}
