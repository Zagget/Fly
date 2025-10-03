using System;
using TMPro;
using UnityEngine;
using UnityEngine.Video;

public class TutorialInPlace : MonoBehaviour
{
    [Header("Tutorial Content")]
    [SerializeField] private string tutorialText;
    [SerializeField] private VideoClip clip;
    [SerializeField] private bool loopVideo;

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private VideoPlayer videoPlayer;

    void Start()
    {
        text.text = tutorialText;

        videoPlayer.clip = clip;
        videoPlayer.isLooping = loopVideo;
    }
}
