using System.Collections.Generic;
using UnityEngine;

public class SortingManager : MonoBehaviour
{
    [SerializeField] Screen screen;

    Dictionary<SortingElement, int> indexDictionary = new();
    List<SortingElement> computerElements = new();

    bool isQuitting = false;

    public static SortingManager Instance {  get; private set; }

    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else Destroy(this);
        Application.quitting += SetQuittingBool;

        foreach (SortingElement element in GetComponentsInChildren<SortingElement>())
        {
            AddToLists(element);
        }

        OrderElements();
    }

    public void SetQuittingBool() { isQuitting = true; }

    public void NewElement(SortingElement element)
    {
        if (indexDictionary.ContainsKey(element)) RemoveFromLists(element);
        AddToLists(element);

        OrderElements();
    }

    private void AddToLists(SortingElement element)
    {
        indexDictionary.Add(element, computerElements.Count);
        computerElements.Add(element);
    }

    public void RemoveElement(SortingElement element)
    {
        if (!indexDictionary.ContainsKey(element)) return;
        RemoveFromLists(element);

        OrderElements();
    }

    private void RemoveFromLists(SortingElement element)
    {
        int index = indexDictionary[element];
        indexDictionary.Remove(element);

        computerElements.RemoveAt(index);
        for (int i = index; i < computerElements.Count; i++)
        {
            indexDictionary[computerElements[i]] = i;
        }
    }

    private void OrderElements()
    {
        if (isQuitting) return;
        for (int i = 0; i < computerElements.Count; i++)
        {
            computerElements[i].layer = i;
            computerElements[i].transform.SetSiblingIndex(i);
        }
    }

    private void OnDestroy()
    {
        Application.quitting -= SetQuittingBool;
    }
}
