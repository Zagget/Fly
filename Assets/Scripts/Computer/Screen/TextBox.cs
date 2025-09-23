using TMPro;
using UnityEngine;

public class TextBox : ComputerElement, IOnPress, IBackSpace
{
    TextMeshPro textMeshPro;

    private void Awake()
    {
        textMeshPro = GetComponent<TextMeshPro>();
        if (textMeshPro== null)
        {
            Debug.LogWarning("Computer TextBox does not have textmeshpro text");
            Destroy(this);
        }
    }

    public override void Selected()
    {

    }

    public void OnPress(char key)
    {
        textMeshPro.text += key;
    }

    public void BackSpace()
    {
        string text = textMeshPro.text;
        if (text.Length < 1) { return; }
        textMeshPro.text = text[..(text.Length - 2)];
    }
}
