using UnityEngine;
using UnityEngine.InputSystem;
public class FlyingState : BasePlayerState
{
    private float stateActiveTime;
    private float timeUntilCanLand = 1.5f;


    private Vector3 leftControllerPosition;
    private Vector3 rightControllerPosition;

    private float maximumControllerHeight; // sets automatically on Entry
    private float controllerLandingHeight;
    private float heightMultiplier = 0.7f;

    RigManager rig;

    // Grabbing when flying?
    public override void HandleGrabLeft(InputAction.CallbackContext context, Grabber leftGrabber)
    {
        base.HandleGrabLeft(context, leftGrabber);
    }

    public override void HandleGrabRight(InputAction.CallbackContext context, Grabber leftGrabber)
    {
        base.HandleGrabLeft(context, leftGrabber);
    }

    public override void HandleDesktopHover(InputAction.CallbackContext context)
    {
        base.HandleDesktopHover(context);

        player.SetState(StateManager.Instance.hoverState);
    }

    public override void Enter(PlayerController player)
    {
        base.Enter(player);
        stateActiveTime = 0;

        maximumControllerHeight = PlayerPrefs.GetFloat(ControllerData.maxControllerHeightKey);

        controllerLandingHeight = 2 - maximumControllerHeight;

        rig = RigManager.instance;
    }

    public override void StateUpdate()
    {
        if (!rig.usingVr) return;

        if (RigManager.instance.usingVr)
        {
            leftControllerPosition = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LHand);
            rightControllerPosition = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RHand);
        }

        stateActiveTime += Time.deltaTime; 

        if (StateManager.Instance.CheckHoverState()) 
        {
            player.SetState(StateManager.Instance.hoverState);
        }
        else if (stateActiveTime > timeUntilCanLand && leftControllerPosition.y < controllerLandingHeight && 
            rightControllerPosition.y < controllerLandingHeight && StateManager.Instance.CheckWalkingState())
        {
            player.SetState(StateManager.Instance.walkingState);
        }
    }
}