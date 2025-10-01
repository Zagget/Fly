using UnityEngine;

public class ClosePopupOriginButton : ComputerElement, ILeftClick
{
    public void LeftClick()
    {
        if (GetSortingElement(out int _).computerElement is PopupPrompt popupPrompt)
        {
            SortingElement originSortingElement = popupPrompt.origin.GetSortingElement(out int _);
            DestroySortingElement(originSortingElement);
            DestroySortingElement(popupPrompt.sortingElement);
        }
    }

    private static void DestroySortingElement(SortingElement originSortingElement)
    {
        SortingManager.Instance.RemoveElement(originSortingElement);
        Destroy(originSortingElement.gameObject);
    }
}
