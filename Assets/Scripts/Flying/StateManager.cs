using System;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    private static StateManager _instance;
    public static StateManager Instance { get { return _instance; } }

    [Header("Walking State")]
    [SerializeField] private float groundRayLength;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float walkingStateMinSpeed;

    [Header("Hover State")]
    private float hoverReqTime = 999999; //prevents user from entering hover without pressing button
    private float hoverMaxSpeed = 3;

    private float hoverTimer;

    private Rigidbody rb;

    [HideInInspector] public HoverState hoverState = new();
    [HideInInspector] public FlyingState flyingState = new();
    [HideInInspector] public WalkingState walkingState = new();
    [HideInInspector] public MenuState menuState = new();
    [HideInInspector] public TutorialState tutorialState = new();
    [HideInInspector] public DashState dashState = new();
    [HideInInspector] public RacePreparationsState racePreparationsState = new();
    [HideInInspector] public RacingState racingState = new();
    [HideInInspector] public MainMenuState mainMenuState = new();

    private bool isWalkingState;

    private float deadZoneSize;

    /// <summary>
    /// Triggers when a state is changed, arg1 is new state, arg2 is last state.
    /// <para> New State, Old State</para>
    /// </summary>
    public event Action<BasePlayerState, BasePlayerState> OnStateChanged;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        rb = RigManager.instance.currentRb;
        deadZoneSize = PlayerPrefs.GetFloat(ControllerData.deadZoneSizeKey);
        if (deadZoneSize == 0) deadZoneSize = 0.3f;
    }

    public void TriggerChangeStateEvent(BasePlayerState newState, BasePlayerState lastState)
    {
        OnStateChanged?.Invoke(newState, lastState);
        if (lastState == menuState)
        {
            deadZoneSize = PlayerPrefs.GetFloat(ControllerData.deadZoneSizeKey);
        }

        if (lastState == hoverState) hoverTimer = 0;
    }

    public bool CheckFlyingState()
    {
        if (IsGrounded() == true) return false;

        return true;
    }

    public bool CheckWalkingState()
    {
        if (isWalkingState == false)
        {
            if (GetSpeedSquared() > walkingStateMinSpeed * walkingStateMinSpeed)
                return false;

            if (IsGrounded())
            {
                isWalkingState = true;
                return true;
            }

            return false;
        }

        if (IsGrounded())
            return true;


        isWalkingState = false;
        return false;
    }

    public void ResetHover()
    {
        hoverTimer = 0;
    }

    public bool CheckHoverState()
    {
        if (IsGrounded()) return false;

        if (!CheckIfControllersAreInDeadZone()) return false;

        if (GetSpeedSquared() < hoverMaxSpeed * hoverMaxSpeed)
        {
            hoverTimer += Time.deltaTime;
        }
        else
        {
            hoverTimer = 0;
        }

        if (hoverTimer >= hoverReqTime)
        {
            return true;
        }

        return false;
    }

    private bool CheckIfControllersAreInDeadZone()
    {
        if (!RigManager.instance.usingVr) return false;



        Vector3 lControllerPos = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LHand);
        Vector3 rControllerPos = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RHand);

        Vector3 deadZone = new Vector3(0, 1, 0);
        Vector3 average = (lControllerPos + rControllerPos) / 2;

        float xValue = Mathf.Abs(average.x);
        float yValue = Mathf.Abs(average.y);
        float zValue = Mathf.Abs(average.z);

        if (xValue < deadZoneSize && yValue < deadZoneSize + 1 && zValue < deadZoneSize)
        {
            return true;
        }

        return false;
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(rb.transform.position, -rb.transform.up, groundRayLength, groundLayer);
    }

    private float GetSpeedSquared()
    {
        return rb.linearVelocity.sqrMagnitude;
    }
}