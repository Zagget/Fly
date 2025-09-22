using UnityEngine;

public class Screen : MonoBehaviour
{
    Vector2 size;
    public Vector2 Size { get { return size; } }

    RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        size = rectTransform.rect.size / 2;
    }
}
