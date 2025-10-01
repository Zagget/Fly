using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    [SerializeField] private TutorialID tutorialToStart;
    private BoxCollider boxCollider;

    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();

        if (boxCollider == null) Debug.LogError("No collider for tutorialtrigger");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            TutorialManager.instance.ActivateTutorial(tutorialToStart);
        }
    }
}