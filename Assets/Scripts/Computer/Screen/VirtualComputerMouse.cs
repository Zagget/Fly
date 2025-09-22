using UnityEngine;

public class VirtualComputerMouse : MonoBehaviour
{
    [SerializeField] Screen screen;
    [SerializeField] LayerMask mask;

    public void Move(Vector3 movement)
    {
        Vector2 newPosition = transform.localPosition;
        newPosition += new Vector2(movement.x, -movement.z);
        newPosition.x = Mathf.Clamp(newPosition.x, -screen.Size.x, screen.Size.x);
        newPosition.y = Mathf.Clamp(newPosition.y, -screen.Size.y, screen.Size.y);
        transform.localPosition = newPosition;
    }

    public ComputerElement Click()
    {
        Collider2D[] colliders = Physics2D.OverlapPointAll(transform.position, mask);
        Vector2Int maxLayers = Vector2Int.zero;
        Vector2Int layers;
        ComputerElement selectElement;
        ComputerElement currentElement = null;

        foreach (Collider2D collider in colliders)
        {
            currentElement = collider.GetComponent<ComputerElement>();
            if (currentElement == null) continue;

            layers = currentElement.GetSortingLayer();
            if (layers.x > maxLayers.x)
            {
                maxLayers = layers;
                selectElement = currentElement;
            }
            else if (layers.y > maxLayers.y)
            {
                maxLayers.y = layers.y;
                selectElement = currentElement;
            }
        }
        return currentElement;
    }
}
