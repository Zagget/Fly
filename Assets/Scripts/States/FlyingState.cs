using UnityEngine;
using UnityEngine.InputSystem;
public class FlyingState : BasePlayerState
{
    private float stateActiveTime;
    private float timeUntilCanLand = 1.5f;


    private Vector3 leftControllerPosition;
    private Vector3 rightControllerPosition;

    private float maximumControllerHeightToLand; // sets automatically on Entry
    private float lessThanMaxHeightPercentage = 0.3f; //30% of arm length up.

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

        maximumControllerHeightToLand = (float.Parse(PlayerPrefs.GetString(ControllerData.maxControllerHeightKey))
            * lessThanMaxHeightPercentage);
    }


    public override void StateUpdate()
    {
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
        else if (stateActiveTime > timeUntilCanLand && leftControllerPosition.y < maximumControllerHeightToLand && 
            rightControllerPosition.y < maximumControllerHeightToLand && StateManager.Instance.CheckWalkingState())
        {
            player.SetState(StateManager.Instance.walkingState);
        }
    }
}