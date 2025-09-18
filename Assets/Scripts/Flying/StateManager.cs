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
    [SerializeField] private float hoverReqTime = 1;
    [SerializeField] private float hoverMaxSpeed = 5;
    [SerializeField] private float hoverDecayRate = 3;

    private float hoverTimer;

    private Rigidbody rb;

    [HideInInspector] public HoverState hoverState = new();
    [HideInInspector] public FlyingState flyingState = new();
    [HideInInspector] public WalkingState walkingState = new();

    private bool isWalkingState;

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

        if (GetSpeedSquared() < hoverMaxSpeed * hoverMaxSpeed)
        {
            hoverTimer += Time.deltaTime;
        }
        else
        {
            hoverTimer -= Time.deltaTime * hoverDecayRate;
            if (hoverTimer < 0) hoverTimer = 0;
        }

        if (hoverTimer >= hoverReqTime)
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