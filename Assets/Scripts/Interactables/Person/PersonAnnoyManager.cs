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

    void OnEnable()
    {
        PersonStates.onPersonChasing += StartChasing;
    }
    void OnDisable()
    {
        PersonStates.onPersonChasing -= StartChasing;
    }

    void StartChasing()
    {
        Debug.Log("start chasing");
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
        personState.ChangeState(BehaviourStates.Sitting);
        triggerCollider.enabled = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && personState.currentState != BehaviourStates.Disabled)
        {
            annoyedAmount++;
            Debug.Log("Annoyed person to: " + annoyedAmount);
            personState.ChangeState(BehaviourStates.Annoyed);
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && personState.currentState != BehaviourStates.Disabled)
        {
            if (annoyedAmount > annoyThreshold)
                personState.ChangeState(BehaviourStates.Chasing);
            else
                personState.ChangeState(BehaviourStates.Neutral);
        }
    }
}
