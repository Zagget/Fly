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
    [SerializeField] Bounds[] movingAreas;
    private int currentBounds;
    [SerializeField] Transform chairSeat;
    [SerializeField] Door door;
    [SerializeField] LightSwitch lightSwitch;
    float targetThreshold = 0.5f;
    PersonStates personStates;
    Vector3 target;
    Vector3 targetDirection;
    Coroutine movementCoroutine;
    Coroutine movementCheckCoroutine;
    static event Action OnTargetReached;
    bool reachedTarget;
    Animator animator;
    Rigidbody rb;
    void Start()
    {
        target = transform.position;
        personStates = GetComponent<PersonStates>();
        animator = GetComponentInChildren<Animator>();
        StopCurrentMoveCoroutine();
        StartCoroutine(RecheckMovement(2));
        rb = GetComponent<Rigidbody>();
    }
    void OnEnable()
    {
        PersonStates.onStateChanged += StopCurrentMoveCoroutine;
        PersonStates.onPersonNeutral += CheckMovement;
        PersonStates.onPersonAnnoyed += CheckMovement;
        PersonStates.onPersonChasing += CheckMovement;
        PersonStates.OnPersonSitting += CheckMovement;
        PersonStates.OnPersonOpenDoor += CheckMovement;
        PersonStates.OnPersonSwitchLight += CheckMovement;
        OnTargetReached += TargetReachedHandler;
    }
    void OnDisable()
    {
        PersonStates.onStateChanged -= StopCurrentMoveCoroutine;
        PersonStates.onPersonNeutral -= CheckMovement;
        PersonStates.onPersonAnnoyed -= CheckMovement;
        PersonStates.onPersonChasing -= CheckMovement;
        PersonStates.OnPersonSitting -= CheckMovement;
        PersonStates.OnPersonOpenDoor -= CheckMovement;
        PersonStates.OnPersonSwitchLight -= CheckMovement;
        OnTargetReached -= TargetReachedHandler;
    }

    IEnumerator MoveToTarget(Vector3 moveTarget)
    {
        targetDirection = moveTarget - transform.position;

        while (Vector3.Distance(transform.position, moveTarget) > targetThreshold)
        {
            float step = moveSpeed * Time.deltaTime;

            rb.position = Vector3.MoveTowards(transform.position, moveTarget, step);
            transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, targetDirection, step * 0.5f, 0));
            yield return null;
        }

        if (!reachedTarget)
            OnTargetReached?.Invoke();
    }

    private void OnCollisionEnter(Collision other)
    {
        Vector3 newTarget = new Vector3(UnityEngine.Random.Range(movingAreas[currentBounds].min.x, movingAreas[currentBounds].max.x), transform.position.y,
                                    UnityEngine.Random.Range(movingAreas[currentBounds].min.z, movingAreas[currentBounds].max.z));
    }

    void CheckMovement()
    {
        Vector3 playerPos = RigManager.instance.pTransform.position;
        GetAllowedBounds();
        switch (personStates.currentState)
        {
            case BehaviourStates.Disabled:
                // No movement
                break;
            case BehaviourStates.Neutral:
                Vector3 newTarget = new Vector3(UnityEngine.Random.Range(movingAreas[currentBounds].min.x, movingAreas[currentBounds].max.x), transform.position.y,
                                    UnityEngine.Random.Range(movingAreas[currentBounds].min.z, movingAreas[currentBounds].max.z));
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
            case BehaviourStates.OpenDoor:
                Vector3 doorInteractPos = door.transform.GetChild(0).position;
                SetTarget(new Vector3(doorInteractPos.x, transform.position.y, doorInteractPos.z));
                break;
            case BehaviourStates.SwitchLight:
                Vector3 lightInteractPos = lightSwitch.transform.parent.GetChild(0).position;
                SetTarget(new Vector3(lightInteractPos.x, transform.position.y, lightInteractPos.z));
                break;
            default:
                Debug.LogWarning("No movement tied to the state: " + personStates.currentState);
                break;
        }
    }

    int GetAllowedBounds()
    {
        //this function is currently hardcoded :)

        int random = UnityEngine.Random.Range(0, 3);
        if (random == 0) return currentBounds;

        switch (currentBounds)
        {
            case 0:
                currentBounds = 1;
                return currentBounds;

            case 1:
                int rnd = UnityEngine.Random.Range(0, 2);
                if (rnd == 0) currentBounds = 0;
                else currentBounds = 2;
                return currentBounds;

            case 2:
                currentBounds = 1;
                return currentBounds;

            default:
                return currentBounds = 2;
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
            case BehaviourStates.OpenDoor:
                StartCoroutine(PersonInteract(door.transform.GetChild(0))); //Get the interact position of the door
                break;
            case BehaviourStates.SwitchLight:
                StartCoroutine(PersonInteract(lightSwitch.transform.parent.GetChild(0)));
                break;
            default:
                break;
        }
    }

    void PersonSit(Transform seat)
    {
        transform.position = new Vector3(seat.position.x, transform.position.y, seat.position.z);
        transform.rotation = seat.rotation;
        animator.SetTrigger("StartSit"); //Play sitting animation
    }
    IEnumerator PersonInteract(Transform interactPosition)
    {
        transform.position = new Vector3(interactPosition.position.x, transform.position.y, interactPosition.position.z);
        transform.rotation = interactPosition.rotation;
        animator.SetTrigger("Interact"); //Play interact animation. Interact event is triggered by animation clip
        yield return AnimationManager.Instance.WaitForAnimation(animator, "Interact");
        personStates.ChangeState(BehaviourStates.Neutral);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        for (int i = 0; i < movingAreas.Length; i++)
        {
            Gizmos.DrawWireCube(movingAreas[i].center, movingAreas[i].size);
        }
        //Gizmos.DrawWireCube(movingAreas[0].center, movingAreas[0].size);
        Gizmos.DrawLine(transform.position, target);
    }
}
