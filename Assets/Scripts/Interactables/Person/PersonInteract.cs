using UnityEngine;

public class PersonInteract : MonoBehaviour
{
    IPersonInteractable currentInteractable;
    void InteractEvent() //Event in the interact animation clip
    {
        if (currentInteractable != null)
        {
            currentInteractable.Interact();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<IPersonInteractable>() != null)
        {
            currentInteractable = other.GetComponent<IPersonInteractable>();
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<IPersonInteractable>() != null)
        {
            currentInteractable = null;
        }
    }
}
