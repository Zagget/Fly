using System;
using System.Collections;
using UnityEngine;

public enum BehaviourStates
{
    Disabled, //used to disable person behaviour
    Standing,
    Wandering,
    Annoyed,
    Chasing,
    Sitting,
    OpenDoor,
    SwitchLight,
    bazooka,
    Ragdoll
}
public class PersonStates : MonoBehaviour
{
    public delegate void OnPersonIgnore();
    public static event OnPersonIgnore onPersonDisabled;
    public static event Action onPersonStanding;
    public delegate void OnPersonWandering();
    public static event OnPersonWandering onPersonWandering;
    public delegate void OnPersonAnnoyed();
    public static event OnPersonAnnoyed onPersonAnnoyed;
    public delegate void OnPersonChasing();
    public static event OnPersonChasing onPersonChasing;
    public static event Action OnPersonSitting;
    public static event Action OnPersonOpenDoor;
    public static event Action OnPersonSwitchLight;
    public static event Action OnPersonRagdoll;
    public static event Action OnRagdollReset;
    public static event Action OnPersonStartBazooka;
    [SerializeField] private BehaviourStates _CurrentState;
    private BehaviourStates preBehaviour;
    public BehaviourStates currentState
    {
        get => _CurrentState;
        set
        {
            if (_CurrentState != value)
            { _CurrentState = value; }
        }
    }
    public delegate void StateChanged(BehaviourStates newState);
    public static StateChanged onStateChanged;
    Animator animator;

    void Awake()
    {
        ChangeState(currentState);
        animator = GetComponentInChildren<Animator>();
    }

    public void ChangeState(BehaviourStates newState)
    {
        if (preBehaviour == BehaviourStates.Sitting &&
            newState != BehaviourStates.Sitting &&
            animator.GetCurrentAnimatorStateInfo(0).IsName("Sitting_Idle_Loop"))
        {
            StartCoroutine(InvokeAfterAnimation(newState, "StopSit", "Sitting_Exit"));
        }
        else if (preBehaviour == BehaviourStates.Ragdoll &&
                 newState != BehaviourStates.Ragdoll)
        {
            //Recover from ragdoll and invoke after done recovering
            Debug.Log("Recovering from ragdoll...");
            OnRagdollReset?.Invoke(); 
            StartCoroutine(InvokeAfterRagdollRecovery(newState));
        }
        else
        {
            InvokeChangeState(newState);
        }
    }

    public IEnumerator InvokeAfterAnimation(BehaviourStates newstate, string triggerName, string animStateName)
    {
        if (AnimationManager.Instance == null)
        {
            Debug.LogError("AnimationManager.Instance is null!");
            yield break;
        }
        Debug.Log("waiting to invoke states until " + animStateName + " is finished");
        animator.SetTrigger(triggerName);
        yield return AnimationManager.Instance.WaitForAnimation(animator, animStateName);
        InvokeChangeState(newstate);

    }
    IEnumerator InvokeAfterRagdollRecovery(BehaviourStates newState)
    {
        PersonMovement pm = GetComponent<PersonMovement>();
        yield return pm.PersonResetFromRagdoll();
        StartCoroutine(InvokeAfterAnimation(newState, "AfterRagdoll", "Crouch"));
    }

    void InvokeChangeState(BehaviourStates newState)
    {
        onStateChanged?.Invoke(newState);

        _CurrentState = newState;
        preBehaviour = newState;
        switch (_CurrentState)
        {
            case BehaviourStates.Disabled:
                onPersonDisabled?.Invoke();
                break;
            case BehaviourStates.Standing:
                onPersonStanding?.Invoke();
                break;
            case BehaviourStates.Wandering:
                onPersonWandering?.Invoke();
                break;
            case BehaviourStates.Annoyed:
                onPersonAnnoyed?.Invoke();
                break;
            case BehaviourStates.Chasing:
                onPersonChasing?.Invoke();
                break;
            case BehaviourStates.Sitting:
                OnPersonSitting?.Invoke();
                break;
            case BehaviourStates.OpenDoor:
                OnPersonOpenDoor?.Invoke();
                break;
            case BehaviourStates.SwitchLight:
                OnPersonSwitchLight?.Invoke();
                break;
            case BehaviourStates.Ragdoll:
                OnPersonRagdoll?.Invoke();
                break;
            case BehaviourStates.bazooka:
                OnPersonStartBazooka?.Invoke();
                break;
            default:
                Debug.LogWarningFormat("Person Behaviour State not recognized!");
                break;
        }
    }

#if UNITY_EDITOR
    private void OnValidate() //Triggers the event when it's changed in the editor
    {
        ChangeState(_CurrentState);
    }
#endif

}
