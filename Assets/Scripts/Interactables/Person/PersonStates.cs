using System;
using UnityEngine;
using UnityEngine.Events;

public enum BehaviourStates
{
    Disabled, //used to disable person behaviour
    Neutral,
    Annoyed,
    Chasing,
    Sitting
}
public class PersonStates : MonoBehaviour
{
    public delegate void OnPersonIgnore();
    public static event OnPersonIgnore onPersonDisabled;
    public delegate void OnPersonNeutral();
    public static event OnPersonNeutral onPersonNeutral;
    public delegate void OnPersonAnnoyed();
    public static event OnPersonAnnoyed onPersonAnnoyed;
    public delegate void OnPersonChasing();
    public static event OnPersonChasing onPersonChasing;
    public static event Action OnPersonSitting;
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
    }
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public void ChangeState(BehaviourStates newState)
    {
        Debug.Log($"old state: {preBehaviour}, new state: {newState}");
        if (preBehaviour == BehaviourStates.Sitting &&
            newState != BehaviourStates.Sitting &&
            animator.GetCurrentAnimatorStateInfo(0).IsName("Sitting_Idle_Loop"))
        {
            animator.SetTrigger("StopSit");
        }
        
        onStateChanged?.Invoke(newState);

        _CurrentState = newState;
        preBehaviour = newState;
        switch (_CurrentState)
        {
            case BehaviourStates.Disabled:
                onPersonDisabled?.Invoke();
                break;
            case BehaviourStates.Neutral:
                onPersonNeutral?.Invoke();
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
