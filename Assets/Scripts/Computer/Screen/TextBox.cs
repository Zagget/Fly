using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextBox : ComputerElement, IOnPress, IBackSpace
{
    [SerializeField] Image image;
    [SerializeField] Color normal;
    [SerializeField] Color selected;
    protected TextMeshProUGUI textMeshPro;

    private void Awake()
    {
        textMeshPro = GetComponent<TextMeshProUGUI>();
        if (textMeshPro == null)
        {
            Debug.LogWarning("Computer TextBox does not have TextMeshProUGUI text");
            Destroy(this);
        }
    }

    public override void Selected()
    {
        image.color = selected;
    }

    public override void Deselected()
    {
        image.color = normal;
    }

    public void OnPress(char key)
    {
        textMeshPro.text += key;
    }

    public void BackSpace()
    {
        string text = textMeshPro.text;
        if (text.Length < 1) { return; }
        textMeshPro.text = text[..(text.Length - 1)];
    }
}
