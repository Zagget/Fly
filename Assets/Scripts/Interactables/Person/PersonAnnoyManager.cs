using System.Collections;
using UnityEngine;

public class PersonAnnoyManager : MonoBehaviour
{
    int annoyedAmount = 0;
    [SerializeField] int annoyThreshold = 10;
    [Range(0, 50)][SerializeField] float chaseDuration = 5f;
    [SerializeField] float chaseSpeed = 13;
    float normalSpeed;
    Collider triggerCollider;
    bool isChasing;
    PersonMovement personMovement;
    PersonStates personState;
    Animator animator;

    int totalTimesAnnoyed;
    [SerializeField] float timeToShootBazooka;
    [SerializeField] int reqTimesAnnoyedForBazooka;
    [SerializeField] GameObject bazookaHolder;
    bool isBazookaCoroutineRunning;
    void Start()
    {
        triggerCollider = GetComponent<Collider>();
        personState = GetComponent<PersonStates>();
        animator = GetComponentInChildren<Animator>();
        personMovement = GetComponent<PersonMovement>();
        normalSpeed = personMovement.moveSpeed;
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
        personMovement.moveSpeed = chaseSpeed;
        isChasing = true;
        animator.SetBool("IsChasing", isChasing);
        StartCoroutine(StopChasing(chaseDuration));
        totalTimesAnnoyed++;
    }

    IEnumerator StopChasing(float chaseTime)
    {
        yield return new WaitForSeconds(chaseTime);
        Debug.Log("stop chasing");
        personMovement.moveSpeed = normalSpeed;
        isChasing = false;
        animator.SetBool("IsChasing", isChasing);
        annoyedAmount = 0;
        personState.ChangeState(BehaviourStates.Sitting);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && personState.currentState != BehaviourStates.Disabled &&
            personState.currentState != BehaviourStates.bazooka)
        {
            if (personState.currentState == BehaviourStates.Chasing)
            {
                animator.SetTrigger("Attack");
            }
            else
            {
                annoyedAmount++;
                Debug.Log("Annoyed person to: " + annoyedAmount);
                personState.ChangeState(BehaviourStates.Annoyed);
            }
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && personState.currentState != BehaviourStates.Disabled)
        {
            if (totalTimesAnnoyed >= reqTimesAnnoyedForBazooka)
            {
                if (personState.currentState != BehaviourStates.bazooka && !isBazookaCoroutineRunning)
                {
                    StartCoroutine(ShootBazooka(timeToShootBazooka));
                    personState.ChangeState(BehaviourStates.bazooka);
                }        
                return;
            }

            if (personState.currentState == BehaviourStates.Chasing) return;

            if (annoyedAmount > annoyThreshold )
            {
                personState.ChangeState(BehaviourStates.Chasing);
            }
            else
            {
                personState.ChangeState(BehaviourStates.Wandering);
            }
        }
    }

    IEnumerator ShootBazooka(float duration)
    {
        float time = 0;
        var rm = RigManager.instance;
        bazookaHolder.SetActive(true);
        animator.Play("Idle_Loop");
        isBazookaCoroutineRunning = true;
        while (time < duration)
        {
            time += Time.deltaTime;
            Vector3 targetDirection = rm.pTransform.position - transform.position;
            transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward,
                new Vector3(targetDirection.x, transform.position.y, targetDirection.z), Time.deltaTime, 0));
            transform.rotation = Quaternion.Euler(transform.rotation.x, 0, transform.rotation.z);

            yield return new WaitForEndOfFrame();
        }

        bazookaHolder.SetActive(false);
        personState.ChangeState(BehaviourStates.Wandering);
        isBazookaCoroutineRunning = false;
    }
}
