using UnityEngine;

public class Door : MonoBehaviour, IPersonInteractable
{
    public void Interact()
    {
        Open();
    }

    public void Open()
    {
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<Rigidbody>().AddForce(-transform.forward * 100);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawCube(transform.GetChild(0).position, Vector3.one);
    }
}
