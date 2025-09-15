using System.Collections;
using UnityEngine;

public class PersonMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 10;
    [SerializeField] Bounds movingArea;
    PersonStates personStates;
    Vector3 target;
    Vector3 targetDirection;
    void Start()
    {
        target = transform.position;
        personStates = GetComponent<PersonStates>();
        StartCoroutine(RecheckMovement(2));
    }
    void OnEnable()
    {
        PersonStates.onPersonNeutral += CheckMovement;
        PersonStates.onPersonAnnoyed += CheckMovement;
        PersonStates.onPersonChasing += CheckMovement;
    }
    void OnDisable()
    {
        PersonStates.onPersonNeutral -= CheckMovement;
        PersonStates.onPersonAnnoyed += CheckMovement;
        PersonStates.onPersonChasing -= CheckMovement;
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

                break;
            case BehaviourStates.Neutral:
                Vector3 newTarget = new Vector3(Random.Range(movingArea.min.x, movingArea.max.x), transform.position.y,
                                    Random.Range(movingArea.min.z, movingArea.max.z));
                SetTarget(newTarget);
                StartCoroutine(RecheckMovement(10));
                break;
            case BehaviourStates.Annoyed:
                Vector3 awayTarget = new Vector3(playerPos.x - transform.position.x, 0,
                                                playerPos.z - transform.position.z) * -1;
                SetTarget(transform.position + awayTarget); //moves away from player
                StartCoroutine(RecheckMovement(1));
                break;
            case BehaviourStates.Chasing:
                SetTarget(new Vector3(playerPos.x, transform.position.y, playerPos.z)); //moves towards player
                StartCoroutine(RecheckMovement(1));
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

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(movingArea.center, movingArea.size);
        Gizmos.DrawLine(transform.position, target);
    }
}
