using UnityEngine;

public class SortingElement : MonoBehaviour
{
    [HideInInspector] public ComputerElement computerElement;

    [HideInInspector] public int layer;

    private void Awake()
    {
        computerElement = GetComponent<ComputerElement>();
        if (computerElement == null) 
        {
            Debug.LogWarning("New sorting element does not have matching computer element");
            Destroy(this);
        }
        computerElement.sortingElement = this;
    }
}
