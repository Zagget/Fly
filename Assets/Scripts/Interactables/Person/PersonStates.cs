using System;
using UnityEngine;
using UnityEngine.Events;

public enum BehaviourStates
{
    Disabled, //used to disable person behaviour
    Neutral,
    Annoyed,
    Chasing
}
public class PersonStates : MonoBehaviour
{
    public delegate void OnPersonIgnore();
    public static OnPersonIgnore onPersonDisabled;
    public delegate void OnPersonNeutral();
    public static OnPersonNeutral onPersonNeutral;
    public delegate void OnPersonAnnoyed();
    public static OnPersonAnnoyed onPersonAnnoyed;
    public delegate void OnPersonChasing();
    public static OnPersonChasing onPersonChasing;
    [SerializeField] private BehaviourStates _CurrentState;
    public BehaviourStates currentState
    {
        get => _CurrentState;
        set
        {
            if (_CurrentState != value)
            { _CurrentState = value; OnStateChanged(_CurrentState); }
        }
    }
    public delegate void StateChanged(BehaviourStates newState);
    public static StateChanged stateChanged;

    void Awake()
    {
        OnStateChanged(currentState);
    }

    public static void OnStateChanged(BehaviourStates newState)
    {
        Debug.Log("State changed to: " + newState);
        stateChanged?.Invoke(newState);
    }

    void OnEnable()
    {
        stateChanged += ChangeState;
    }
    void OnDisable()
    {
        stateChanged -= ChangeState;
    }

    private void ChangeState(BehaviourStates newState)
    {
        _CurrentState = newState;
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
