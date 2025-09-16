using System.Collections;
using UnityEngine;

public class PersonAnnoyManager : MonoBehaviour
{
    int annoyedAmount = 0;
    [SerializeField] int annoyThreshold = 10;
    [Range(0, 20)][SerializeField] float chaseDuration = 5f;
    Collider triggerCollider;
    bool isChasing;
    PersonStates personState;

    void Start()
    {
        triggerCollider = GetComponent<Collider>();
        personState = GetComponent<PersonStates>();
    }

    void StartChasing()
    {
        Debug.Log("start chasing");
        PersonStates.OnStateChanged(BehaviourStates.Chasing);
        isChasing = true;
        triggerCollider.enabled = false;
        StartCoroutine(StopChasing(chaseDuration));
    }

    IEnumerator StopChasing(float chaseTime)
    {
        yield return new WaitForSeconds(chaseTime);
        Debug.Log("stop chasing");
        isChasing = false;
        annoyedAmount = 0;
        PersonStates.OnStateChanged(BehaviourStates.Neutral);
        triggerCollider.enabled = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && personState.currentState != BehaviourStates.Disabled)
        {
            annoyedAmount++;
            Debug.Log("Annoyed person to: " + annoyedAmount);
            PersonStates.OnStateChanged(BehaviourStates.Annoyed);
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && personState.currentState != BehaviourStates.Disabled)
        {
            if (annoyedAmount > annoyThreshold)
                StartChasing();
            else
                PersonStates.OnStateChanged(BehaviourStates.Neutral);
        }
    }
}
