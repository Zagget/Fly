using UnityEngine;

public class ComputerElement : MonoBehaviour 
{
    [SerializeField] ComputerElement parentElement;
    public ComputerElement ParentElement { get {  return parentElement; } }
    [HideInInspector] public SortingElement sortingElement;

    public void InternalSelected()
    {
        SortingManager.Instance.NewElement(GetSortingElement(out int _));
        Selected();
    }

    public virtual void Selected() { }
    public virtual void Deselected() { }

    public SortingElement GetSortingElement(out int depth)
    {
        ComputerElement currentElement = this;
        depth = 0;
        while (currentElement.parentElement != null)
        {
            currentElement = currentElement.parentElement;
            depth++;
        }
        return currentElement.sortingElement;
    }

    public Vector2Int GetSortingLayer()
    {
        SortingElement element = GetSortingElement(out int depth);
        if (element == null) return Vector2Int.zero;
        return new Vector2Int(element.layer, depth);
    }

    private void OnDisable()
    {
        if (sortingElement != null) SortingManager.Instance.RemoveElement(sortingElement); 
    }
}
