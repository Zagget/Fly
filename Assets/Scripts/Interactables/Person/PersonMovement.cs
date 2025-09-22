using System;
using System.Collections;
using UnityEngine;

public class PersonMovement : MonoBehaviour
{
    [Header("Person Settings")]
    public float moveSpeed = 10;
    [Range(0.1f, 50)][SerializeField] float passiveMovingFrequency = 10;
    [Range(0.1f, 50)][SerializeField] float activeMovingFrequency = 0.1f;
    [Header("Movement Area")]
    [SerializeField] Bounds movingArea;
    [SerializeField] Transform chairSeat;
    float targetThreshold = 0.5f;
    PersonStates personStates;
    Vector3 target;
    Vector3 targetDirection;
    Coroutine movementCoroutine;
    Coroutine movementCheckCoroutine;
    static event Action OnTargetReached;
    bool reachedTarget;
    Animator animator;
    void Start()
    {
        target = transform.position;
        personStates = GetComponent<PersonStates>();
        animator = GetComponentInChildren<Animator>();
        StopCurrentMoveCoroutine();
        StartCoroutine(RecheckMovement(2));
    }
    void OnEnable()
    {
        PersonStates.onStateChanged += StopCurrentMoveCoroutine;
        PersonStates.onPersonNeutral += CheckMovement;
        PersonStates.onPersonAnnoyed += CheckMovement;
        PersonStates.onPersonChasing += CheckMovement;
        PersonStates.OnPersonSitting += CheckMovement;
        OnTargetReached += TargetReachedHandler;
    }
    void OnDisable()
    {
        PersonStates.onStateChanged -= StopCurrentMoveCoroutine;
        PersonStates.onPersonNeutral -= CheckMovement;
        PersonStates.onPersonAnnoyed -= CheckMovement;
        PersonStates.onPersonChasing -= CheckMovement;
        PersonStates.OnPersonSitting -= CheckMovement;
        OnTargetReached -= TargetReachedHandler;
    }

    IEnumerator MoveToTarget(Vector3 moveTarget)
    {
        targetDirection = moveTarget - transform.position;

        while (Vector3.Distance(transform.position, moveTarget) > targetThreshold)
        {
            float step = moveSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, moveTarget, step);
            transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, targetDirection, step * 0.5f, 0));
            yield return null;
        }
        
        if (!reachedTarget)
            OnTargetReached?.Invoke();
    }

    void CheckMovement()
    {
        Vector3 playerPos = RigManager.instance.pTransform.position;

        switch (personStates.currentState)
        {
            case BehaviourStates.Disabled:
                // No movement
                break;
            case BehaviourStates.Neutral:
                Vector3 newTarget = new Vector3(UnityEngine.Random.Range(movingArea.min.x, movingArea.max.x), transform.position.y,
                                    UnityEngine.Random.Range(movingArea.min.z, movingArea.max.z));
                SetTarget(newTarget); //moves to random point inside movingArea
                movementCheckCoroutine = StartCoroutine(RecheckMovement(passiveMovingFrequency));
                break;
            case BehaviourStates.Annoyed:
                Vector3 awayTarget = new Vector3(playerPos.x - transform.position.x, 0,
                                                playerPos.z - transform.position.z) * -1;
                SetTarget(transform.position + awayTarget); //moves away from player
                movementCheckCoroutine = StartCoroutine(RecheckMovement(activeMovingFrequency));
                break;
            case BehaviourStates.Chasing:
                SetTarget(new Vector3(playerPos.x, transform.position.y, playerPos.z)); //moves towards player
                movementCheckCoroutine = StartCoroutine(RecheckMovement(activeMovingFrequency));
                break;
            case BehaviourStates.Sitting:
                SetTarget(new Vector3(chairSeat.position.x, transform.position.y, chairSeat.position.z)); //move to chair
                break;
            default:
                Debug.LogWarning("No movement tied to the state: " + personStates.currentState);
                break;
        }
    }

    void SetTarget(Vector3 moveTarget)
    {
        target = moveTarget;
        reachedTarget = false;
        
        if (movementCoroutine != null)
        {
            StopCoroutine(movementCoroutine);
            movementCoroutine = null;
        }
        animator.SetBool("IsWalking", !reachedTarget);
        movementCoroutine = StartCoroutine(MoveToTarget(target));
    }

    IEnumerator RecheckMovement(float time)
    {
        yield return new WaitForSeconds(time);
        CheckMovement();
    }

    void StopCurrentMoveCoroutine(BehaviourStates behaviourStates = BehaviourStates.Neutral)
    {
        StopAllCoroutines();
        movementCoroutine = null;
        movementCheckCoroutine = null;
    }

    void TargetReachedHandler()
    {
        reachedTarget = true;
        animator.SetBool("IsWalking", !reachedTarget);
        StopCoroutine(movementCoroutine);
        movementCoroutine = null;

        switch (personStates.currentState)
        {
            case BehaviourStates.Neutral:
                break;
            case BehaviourStates.Annoyed:
                break;
            case BehaviourStates.Chasing:
                //reached player
                Debug.Log("Person reached player");
                personStates.ChangeState(BehaviourStates.Neutral); //temp
                break;
            case BehaviourStates.Sitting:
                PersonSit(chairSeat);
                break;
            default:
                break;
        }
    }

    void PersonSit(Transform seat)
    {
        transform.position = new Vector3(seat.position.x, transform.position.y, seat.position.z);
        transform.rotation = seat.rotation;
        //Play sitting animation
        animator.SetTrigger("StartSit");
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(movingArea.center, movingArea.size);
        Gizmos.DrawLine(transform.position, target);
    }
}
