using System.Collections;
using UnityEngine;

public class PersonMovement : MonoBehaviour
{
    [Header("Person Settings")]
    [SerializeField] float moveSpeed = 10;
    [Range(0.1f, 50)][SerializeField] float passiveMovingFrequency = 10;
    [Range(0.1f, 50)][SerializeField] float activeMovingFrequency = 1;
    [Header("Movement Area")]
    [SerializeField] Bounds movingArea;

    PersonStates personStates;
    Vector3 target;
    Vector3 targetDirection;
    Coroutine movementCoroutine;
    void Start()
    {
        target = transform.position;
        personStates = GetComponent<PersonStates>();
        StopCurrentMoveCoroutine();
        StartCoroutine(RecheckMovement(2));
    }
    void OnEnable()
    {
        PersonStates.onPersonNeutral += CheckMovement;
        PersonStates.onPersonAnnoyed += CheckMovement;
        PersonStates.onPersonChasing += CheckMovement;
        PersonStates.stateChanged += StopCurrentMoveCoroutine;
    }
    void OnDisable()
    {
        PersonStates.onPersonNeutral -= CheckMovement;
        PersonStates.onPersonAnnoyed -= CheckMovement;
        PersonStates.onPersonChasing -= CheckMovement;
        PersonStates.stateChanged -= StopCurrentMoveCoroutine;
    }

    void FixedUpdate()
    {
        if (personStates.currentState != BehaviourStates.Disabled)
        {
            if (!Mathf.Approximately(transform.position.x, target.x) || !Mathf.Approximately(transform.position.z, target.z))
            {
                float step = moveSpeed * Time.fixedDeltaTime;
                transform.position = Vector3.MoveTowards(transform.position, target, step);
                transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, targetDirection, step, 0));
            }
        }
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
                Vector3 newTarget = new Vector3(Random.Range(movingArea.min.x, movingArea.max.x), transform.position.y,
                                    Random.Range(movingArea.min.z, movingArea.max.z));
                SetTarget(newTarget); //moves to random point inside movingArea
                movementCoroutine = StartCoroutine(RecheckMovement(passiveMovingFrequency));
                break;
            case BehaviourStates.Annoyed:
                Vector3 awayTarget = new Vector3(playerPos.x - transform.position.x, 0,
                                                playerPos.z - transform.position.z) * -1;
                SetTarget(transform.position + awayTarget); //moves away from player
                movementCoroutine = StartCoroutine(RecheckMovement(activeMovingFrequency));
                break;
            case BehaviourStates.Chasing:
                SetTarget(new Vector3(playerPos.x, transform.position.y, playerPos.z)); //moves towards player
                movementCoroutine = StartCoroutine(RecheckMovement(activeMovingFrequency));
                break;
            default:

                break;
        }
    }

    void SetTarget(Vector3 moveTarget)
    {
        target = moveTarget;
        targetDirection = moveTarget - transform.position;
    }

    IEnumerator RecheckMovement(float time)
    {
        yield return new WaitForSeconds(time);
        CheckMovement();
    }

    void StopCurrentMoveCoroutine(BehaviourStates behaviourStates = BehaviourStates.Neutral)
    {
        if (movementCoroutine != null)
        {
            StopCoroutine(movementCoroutine);
            movementCoroutine = null;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(movingArea.center, movingArea.size);
        Gizmos.DrawLine(transform.position, target);
    }
}
