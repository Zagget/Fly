using UnityEngine;
using System.Collections.Generic;

public class MessageProfile : ComputerElement, ILeftClick
{
    [SerializeField] MessagingApplication MessagingApp;
    [SerializeField] List<Message> messages;

    public void LeftClick()
    {
        MessagingApp.Populate(messages);
    }
}
