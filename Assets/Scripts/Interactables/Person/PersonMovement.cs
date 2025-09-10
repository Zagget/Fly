using System.Collections;
using UnityEngine;

public class PersonMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 10;
    [SerializeField] Bounds movingArea;
    PersonStates personStates;
    Vector3 target;
    void Start()
    {
        target = transform.position;
        personStates = GetComponent<PersonStates>();
        StartCoroutine(RecheckMovement(2));
    }
    void OnEnable()
    {
        PersonStates.onPersonNeutral += CheckMovement;
        PersonStates.onPersonChasing += CheckMovement;
    }
    void OnDisable()
    {
        PersonStates.onPersonNeutral -= CheckMovement;
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
                Vector3 targetDirection = target - transform.position;
                transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, targetDirection, step, 0));
            }
        }
    }

    void CheckMovement()
    {
        switch (personStates.currentState)
        {
            case BehaviourStates.Disabled:

                break;
            case BehaviourStates.Neutral:
                target = new Vector3(Random.Range(movingArea.min.x, movingArea.max.x), transform.position.y,
                                    Random.Range(movingArea.min.z, movingArea.max.z));
                StartCoroutine(RecheckMovement(10));
                break;
            case BehaviourStates.Annoyed:

                break;
            case BehaviourStates.Chasing:
                Vector3 playerPos = RigManager.instance.pTransform.position;
                target = new Vector3(playerPos.x, transform.position.y, playerPos.z);
                StartCoroutine(RecheckMovement(1));
                break;
            default:

                break;
        }
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
    }
}
