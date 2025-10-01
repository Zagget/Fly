using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MessagingApplication : MonoBehaviour
{
    [SerializeField] GameObject fromMessagePrefab;
    [SerializeField] GameObject toMessagePrefab;
    
    [Header("")]
    [SerializeField] Transform textBox;
    [SerializeField] GameObject messagingArea;

    private void Start()
    {
        messagingArea.SetActive(false);
    }

    public void Populate(List<Message> messages)
    {
        Clear();

        messagingArea.SetActive(true);
        foreach (Message message in messages)
        {
            Add(message);
        }
    }

    private void Clear()
    {
        int childCount = textBox.childCount;
        for (int i = 0; i < childCount; i++)
        {
            Destroy(textBox.GetChild(i).gameObject);
        }
    }

    public void Add(Message message)
    {
        GameObject messagePrefab;
        if (message.isFrom) messagePrefab = fromMessagePrefab;
        else messagePrefab = toMessagePrefab;
        GameObject currentGameObject = Instantiate(messagePrefab, textBox);
        currentGameObject.transform.SetAsFirstSibling();
        TextMeshProUGUI currentText = currentGameObject.GetComponentInChildren<TextMeshProUGUI>();
        Debug.Log(currentText, currentText);
        if (currentText != null) { currentText.text = message.message; }
    }
}

[System.Serializable]
public struct Message
{
    public Message(string message, bool isFrom)
    {
        this.message = message;
        this.isFrom = isFrom;
    }

    public string message;
    public bool isFrom;
}
