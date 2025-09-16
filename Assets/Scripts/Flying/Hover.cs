using UnityEngine;

public class Hover : MonoBehaviour
{

    [SerializeField] private float groundRayLength;

    [SerializeField] private float lowSpeedLimit;

    [SerializeField] private LayerMask groundLayer;

    [SerializeField] private float maxGroundTilt;

    private Rigidbody rb;

    private RaycastHit rayHit;

    [HideInInspector] public bool isOnGround;
    [HideInInspector] public bool isHovering;

    private PlayerController pController;

    private float hoverTimer;
    private float hoverReqTime = 3;
    private float hoverMaxSpeed = 5;

    private bool tempHasChangedHoverState; //this needs to be switches to a get current state from player controller.
    //it needs to be here to not change state every frame.

    private void Start()
    {
        rb = RigManager.instance.currentRb;
        pController = GetComponent<PlayerController>();
    }

    private void Update()
    {

        if (CheckIfLowSpeed() == false && isOnGround == false)
        {
            return;
        }

        if (checkIfNearGround())
        {
            if (!isOnGround)
            {
                LandOnGround();
                tempHasChangedHoverState = false;
            }
        }
        else
        {
            StartFlying();
            tempHasChangedHoverState = false;
        }

        isHovering = CheckIfHovering();

        if (isHovering && !tempHasChangedHoverState)
        {
            pController.SetState(new HoverState());
            tempHasChangedHoverState = true;
        }
    }

    private void StartFlying()
    {
        //rb.transform.rotation =
        //    Quaternion.FromToRotation(rb.transform.up, new Vector3(0, 1, 0)) * rb.transform.rotation;
        //isOnGround = false;
        pController.SetState(new FlyingState());
    }

    private void LandOnGround()
    {
        //rb.rotation = Quaternion.Lerp(rb.rotation,
        //    Quaternion.Euler(1 - rayHit.normal.y, rb.rotation.y, rb.rotation.z), Time.fixedDeltaTime);

        //rb.transform.rotation =
        //    Quaternion.FromToRotation(rb.transform.up, rayHit.normal) * rb.transform.rotation;
        //isOnGround = true;

        pController.SetState(new WalkingState());
    }

    private bool CheckIfHovering()
    {
        if (isOnGround == true)
        {
            hoverTimer = 0;
            return false;
        }

        if (rb.linearVelocity.sqrMagnitude < hoverMaxSpeed * hoverMaxSpeed)
        {
            hoverTimer += Time.deltaTime;
        }
        else
        {
            hoverTimer = 0;
            return false;
        }

        if (hoverTimer > hoverReqTime)
        {
            return true;
        }

        return false;

    }

    private bool checkIfNearGround()
    {
        RaycastHit[] downHits;
        RaycastHit[] forwardHits;

        downHits = Physics.RaycastAll(rb.transform.position, -rb.transform.up, groundRayLength, groundLayer);
        forwardHits = Physics.RaycastAll(rb.transform.position, rb.transform.forward, groundRayLength, groundLayer);


        //For normals on Y 1 is completely flat while 0.8 would be a decent slope
        if (downHits.Length > 0 && downHits[0].normal.y > 1 - maxGroundTilt && downHits[0].normal.y < 1 + maxGroundTilt)
        {
            rayHit = downHits[0];
            return true;
        }

        if (forwardHits.Length > 0 && forwardHits[0].normal.y > 1 - maxGroundTilt && forwardHits[0].normal.y < 1 - maxGroundTilt)
        {
            rayHit = forwardHits[0];
            return true;
        }

        return false;
    }
    private bool CheckIfLowSpeed()
    {
        if (rb.linearVelocity.sqrMagnitude < lowSpeedLimit * lowSpeedLimit)
            return true;

        return false;
    }
}