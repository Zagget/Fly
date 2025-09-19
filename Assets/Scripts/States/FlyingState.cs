using UnityEngine;
using UnityEngine.InputSystem;
public class FlyingState : BasePlayerState
{
    private float stateActiveTime;
    private float timeUntilCanLand = 1.5f;


    private Vector3 leftControllerPosition;
    private Vector3 rightControllerPosition;

    private float maximumControllerHeightToLand; // sets automatically on Entry
    private float lessThanMaxHeightPercentage = 0.8f; //20% less then max arm height of players.
    public override void HandleMovement(InputAction.CallbackContext context, FloatingMovement movement)
    {
        
    }

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

        maximumControllerHeightToLand = float.Parse(PlayerPrefs.GetString(ControllerData.deadZoneCenterKey))
            / lessThanMaxHeightPercentage;
    }


    public override void StateUpdate()
    {
        leftControllerPosition = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LHand);
        rightControllerPosition = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RHand);

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