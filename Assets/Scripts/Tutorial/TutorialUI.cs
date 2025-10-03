using UnityEngine;
using TMPro;
using UnityEngine.Video;
using System.Collections;

public class TutorialUI : MonoBehaviour
{
    [SerializeField] private float duration = 2f;

    [SerializeField] private CanvasGroup group;
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

        Debug.Log($"TutorialManager SetTutorial: header: {data.header}");

        group.alpha = 0;
        tutorialPanel.SetActive(true);
        StartCoroutine(FadeIn(duration));
    }

    private IEnumerator FadeIn(float duration)
    {
        float elapsed = 0;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            group.alpha = Mathf.Lerp(0, 1, elapsed / duration);
        }

        yield break;
    }

    public void PressResume()
    {
        tutorialPanel.SetActive(false);
        StateManager.Instance.tutorialState.ExitMenu();
    }

    public void PressSkip()
    {
        tutorialPanel.SetActive(false);
        TutorialManager.instance.SkipTutorials();
        StateManager.Instance.tutorialState.ExitMenu();
    }
}