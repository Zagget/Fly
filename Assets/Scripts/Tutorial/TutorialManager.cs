using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    private static TutorialManager _instance;
    public static TutorialManager instance { get { return _instance; } }

    public event Action<TutorialData> InTutorial;

    [SerializeField] private bool useTutorial;

    private TutorialData[] tutorials;

    private Dictionary<TutorialID, TutorialData> tutorialsById;
    private Dictionary<TutorialData, bool> usedTutorial;

    private PlayerController controller;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
            Destroy(gameObject);
    }

    void Start()
    {
        if (!useTutorial) return;

        Debug.Log("TutorialManager Tutorials are being used");
        controller = FindFirstObjectByType<PlayerController>();
        LoadAllTutorials();

        StartCoroutine(FirstTutorial(2f));
    }

    private IEnumerator FirstTutorial(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        ActivateTutorial(TutorialID.Flying);
    }

    private void LoadAllTutorials()
    {
        tutorials = Resources.LoadAll<TutorialData>("Tutorials");

        tutorialsById = new Dictionary<TutorialID, TutorialData>();
        usedTutorial = new Dictionary<TutorialData, bool>();

        foreach (var tutorial in tutorials)
        {
            if (!tutorialsById.ContainsKey(tutorial.id))
            {
                tutorialsById[tutorial.id] = tutorial;
                usedTutorial[tutorial] = false;
            }
            else
            {
                Debug.LogWarning($"TutorialManager Duplicate TutorialID: {tutorial.id}");
            }
        }
    }

    public void ActivateTutorial(TutorialID id)
    {
        if (!useTutorial)
        {
            Debug.Log("TutorialManager Tutorial is skipped not activating");
            return;
        }
        if (!tutorialsById.ContainsKey(id))
        {
            Debug.LogWarning($"TutorialManager Tutorial {id} not found!");
            return;
        }

        ActivateTutorial(tutorialsById[id]);
    }


    private void ActivateTutorial(TutorialData tutorial)
    {
        if (tutorial == null)
        {
            Debug.Log("TutorialManager Tutorial is null");
            return;
        }

        if (!usedTutorial.ContainsKey(tutorial))
        {
            Debug.Log("TutorialManager tutorial not found in tutorials");
            return;
        }

        if (usedTutorial[tutorial])
        {
            Debug.Log("TutorialManager Tutorials already been used");
            return;
        }

        usedTutorial[tutorial] = true;

        InTutorial?.Invoke(tutorial);
        controller.SetState(StateManager.Instance.tutorialState);

        Debug.Log($"TutorialManager Activated tutorial: {tutorial.header}");
    }

    public void SkipTutorials()
    {
        useTutorial = false;
    }
}