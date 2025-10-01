using UnityEngine;

public class VirtualComputerMouse : MonoBehaviour
{
    [SerializeField] Screen screen;
    [SerializeField] LayerMask mask;
    [SerializeField] float radius;

    public void Move(Vector3 movement)
    {
        Vector2 newPosition = transform.localPosition;
        newPosition += new Vector2(movement.z, -movement.x);
        newPosition.x = Mathf.Clamp(newPosition.x, -screen.Size.x, screen.Size.x);
        newPosition.y = Mathf.Clamp(newPosition.y, -screen.Size.y, screen.Size.y);
        transform.localPosition = newPosition;
    }

    public ComputerElement Click()
    {

        Collider[] colliders = Physics.OverlapSphere(transform.position, radius, mask, QueryTriggerInteraction.Collide);

        Debug.DrawRay(transform.position - screen.transform.forward, screen.transform.forward * 2, Color.red, 20);
        Vector2Int maxLayers = -Vector2Int.one;
        Vector2Int layers;
        ComputerElement selectElement = null;
        ComputerElement currentElement;

        foreach (Collider collider in colliders)
        {
            currentElement = collider.GetComponent<ComputerElement>();
            if (currentElement == null) continue;
            layers = currentElement.GetSortingLayer();
            if (layers.x > maxLayers.x)
            {
                maxLayers = layers;
                selectElement = currentElement;
            }
            else if (layers.y > maxLayers.y && layers.x == maxLayers.x)
            {
                maxLayers.y = layers.y;
                selectElement = currentElement;
            }
        }
        return selectElement;
    }
}
