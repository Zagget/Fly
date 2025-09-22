using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class VRButton : MonoBehaviour
{
    public UnityEvent onPress;
    private BoxCollider boxCollider;
    private RectTransform rectTransform;
    private Image image;

    private int hoverCount = 0;

    void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
        rectTransform = GetComponent<RectTransform>();

        image = GetComponent<Image>();

        if (image == null) Debug.LogWarning("VRButton: No Image found on this object or its children.");

        UpdateCollider();
    }

    void Start()
    {
        image.color = Color.white;
    }

    public void UpdateCollider()
    {
        if (boxCollider == null || rectTransform == null) return;

        Vector3 size = new Vector3(rectTransform.rect.width, rectTransform.rect.height, 0.01f);
        boxCollider.size = size;
        boxCollider.center = Vector3.zero;
    }

    public void Press()
    {
        onPress.Invoke();
    }

    public void Hover(bool hovering)
    {
        if (hovering)
        {
            image.color = Color.yellow;
        }
        else
        {
            image.color = Color.white;
        }
    }
}