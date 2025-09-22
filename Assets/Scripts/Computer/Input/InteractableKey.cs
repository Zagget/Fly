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
        Vector3 newPos = startPos + parent.linearVelocity;
        newPos.y = Mathf.Clamp(transform.localPosition.y + pushback * Time.deltaTime, startPos.y - distance, startPos.y);
        rb.position = transform.parent.TransformPoint(newPos);

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
        rb.linearVelocity = Vector3.zero;
    }

    protected abstract void OnPress();
}
