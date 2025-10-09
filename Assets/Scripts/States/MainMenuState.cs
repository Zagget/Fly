using UnityEngine.InputSystem;

public class MainMenuState : BasePlayerState
{
    private RigManager rig;
    public override void Enter(PlayerController player)
    {
        base.Enter(player);
        rig = RigManager.instance;

        if (rig.usingVr)
        {
            player.leftPointer.SetVisible(true);
            player.rightPointer.SetVisible(true);
        }
    }

    public override void HandleTriggerRight(InputAction.CallbackContext context)
    {
        player.rightPointer.OnPress(context);
    }
}
