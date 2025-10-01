using UnityEngine;

public class CloseButton : ComputerElement, ILeftClick
{
    public void LeftClick()
    {
        SortingElement sortingElement = GetSortingElement(out int _);
        SortingManager.Instance.RemoveElement(sortingElement);
        Destroy(sortingElement.gameObject);
    }
}
