using UnityEngine;

[CreateAssetMenu(fileName = "new Dash", menuName = "Scriptable Objects/Powers/Dash Power")]
public class DashPower : BaseInstantPower
{
    [SerializeField] float speedFactor = 10;

    BasePlayerState lastState;

    public override void Start()
    {
        StateManager.Instance.OnStateChanged -= OnStateChange;
        StateManager.Instance.OnStateChanged += OnStateChange;
        playerController.SetState(StateManager.Instance.dashState);

        currentCharge = Mathf.Clamp(currentCharge, 0f, maximumCharge);
        float dashSpeed = speedFactor * (currentCharge / maximumCharge);
        rigManager.currentRb.linearVelocity = rigManager.eyeAnchor.transform.forward * dashSpeed;
    }

    public override void End()
    {
        StateManager.Instance.OnStateChanged -= OnStateChange;
        playerController.SetState(lastState);
    }

    public void OnStateChange(BasePlayerState newState, BasePlayerState lastState)
    {
        if (!(lastState is DashState)) this.lastState = lastState;
    }
}