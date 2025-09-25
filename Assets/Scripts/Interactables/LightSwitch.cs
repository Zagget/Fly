using UnityEngine;

public class LightSwitch : MonoBehaviour, IPersonInteractable
{
    [SerializeField] Light[] connectedLights;

    public void Interact()
    {
        ToogleLights();
    }

    void ToogleLights()
    {
        foreach (Light item in connectedLights)
        {
            item.enabled = !item.enabled;
        }
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.DrawCube(transform.GetChild(0).position, Vector3.one);
        foreach (Light item in connectedLights)
        {
            Gizmos.color = item.color;
            Gizmos.DrawLine(transform.position, item.transform.position);
        }
    }

    //TODO: add hingejoint that sticks to either side, so player also can switch light by pushing it
}
