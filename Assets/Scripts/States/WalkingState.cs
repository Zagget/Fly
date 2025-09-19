using UnityEngine.InputSystem;
using UnityEngine;
public class WalkingState : BasePlayerState
{
    private Vector3 leftControllerPosition;
    private Vector3 rightControllerPosition;
    private float minimumHeightToTakeOff;

    public override void HandleWalkingMovement(InputAction.CallbackContext context, WalkingMovement walkingMovement)
    {
        walkingMovement.WalkingInput(context);
    }

    public override void HandlePrimaryButton(InputAction.CallbackContext context)
    {
        player.SetState(StateManager.Instance.flyingState);
    }

    public override void Enter(PlayerController player)
    {
        base.Enter(player);
        minimumHeightToTakeOff = PlayerPrefs.GetFloat(ControllerData.maxControllerHeightKey);
    }

    public override void StateUpdate()
    {
        leftControllerPosition = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LHand);
        rightControllerPosition = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RHand);

        if (rightControllerPosition.y > minimumHeightToTakeOff && leftControllerPosition.y > minimumHeightToTakeOff)
        {
            player.SetState(StateManager.Instance.flyingState);
        }

        if (StateManager.Instance.CheckFlyingState())
        {
            player.SetState(StateManager.Instance.flyingState);
        }
    }
}