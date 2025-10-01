using UnityEngine;

public class PromptingCloseButton : ComputerElement, ILeftClick
{
    [SerializeField] PopupPrompt prompt;
    PopupPrompt currentPopup;
    
    public void LeftClick()
    {
        if (currentPopup != null) SortingManager.Instance.NewElement(currentPopup.GetSortingElement(out int _));
        else if (Instantiate(prompt, SortingManager.Instance.transform).TryGetComponent<PopupPrompt>(out PopupPrompt popupPrompt))
        {
            SortingManager.Instance.NewElement(popupPrompt.sortingElement);
            popupPrompt.origin = this;
            currentPopup = popupPrompt;
        }
    }

}
