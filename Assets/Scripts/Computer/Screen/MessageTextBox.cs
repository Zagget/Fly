using UnityEngine;

public class MessageTextBox : TextBox, IEnter
{
    [SerializeField] MessagingApplication messagingApp;

    public void Enter()
    {
        if (messagingApp == null) return;
        messagingApp.Add(new Message(textMeshPro.text, false));
        textMeshPro.text = "";
    }
}
