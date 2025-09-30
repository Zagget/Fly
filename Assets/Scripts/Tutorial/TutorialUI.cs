using UnityEngine;
using TMPro;
using UnityEngine.Video;
using Unity.VisualScripting;

public class TutorialUI : MonoBehaviour
{
    [SerializeField] private GameObject tutorialPanel;

    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private TextMeshProUGUI tutorialHeader;
    [SerializeField] private TextMeshProUGUI tutorialDescription;

    void Start()
    {
        tutorialPanel.SetActive(false);

        TutorialManager.instance.InTutorial += SetTutorial;
    }

    public void SetTutorial(TutorialData data)
    {
        if (data.clip != null)
            videoPlayer.clip = data.clip;

        if (data.header != null)
            tutorialHeader.text = data.header;

        if (data.description != null)
            tutorialDescription.text = data.description;

        tutorialPanel.SetActive(true);
    }

    public void PressResume()
    {
        tutorialPanel.SetActive(false);
        StateManager.Instance.tutorialState.ExitMenu();
    }
}