using UnityEngine;

public class Icon : ComputerElement, ILeftClick
{
    [SerializeField] GameObject Window;
    [SerializeField] float doubleClickInterval;

    float lastClick;

    public void LeftClick()
    {
        if (Time.time - lastClick < doubleClickInterval)
        {
            if (Instantiate(Window, SortingManager.Instance.transform).TryGetComponent<SortingElement>(out SortingElement sortingElement))
            {
                SortingManager.Instance.NewElement(sortingElement);
            }
        }
        lastClick = Time.time;
    }
}
