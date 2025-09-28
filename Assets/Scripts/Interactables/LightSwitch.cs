using UnityEngine;

public class LightSwitch : MonoBehaviour, IPersonInteractable
{
    [SerializeField] Light[] connectedLights;
    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Interact()
    {
        FlipSwitch();
    }

    void FlipSwitch()
    {
        if (transform.localRotation.x < 0)
        {
            rb.AddRelativeTorque(Vector3.right * 100);
        }
        else
        {
            rb.AddRelativeTorque(Vector3.left * 100);
        }
    }

    public void ToggleLights() // triggered by SwitchTrigger when the switch touches the trigger
    {
        foreach (Light item in connectedLights)
        {
            item.enabled = !item.enabled;
        }
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.DrawCube(transform.parent.GetChild(0).position, Vector3.one);
        foreach (Light item in connectedLights)
        {
            Gizmos.color = item.color;
            Gizmos.DrawLine(transform.position, item.transform.position);
        }
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, GetComponent<HingeJoint>().axis);
    }
}
