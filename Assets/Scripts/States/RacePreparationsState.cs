using UnityEngine;

public class RacePreparationsState : BasePlayerState
{
    Transform eyeAnchor;

    RaceHolder raceHolder;
    RaceUI raceUI;
    Vector3 checkpointForward;
    float minDotProduct;
    float timeToStart;
    float timeForCountdown;

    float timeHolding;
    bool inCountdown = false;

    public override void Enter(PlayerController player)
    {
        base.Enter(player);
        eyeAnchor = RigManager.instance.eyeAnchor;
        raceHolder = RaceHolder.current;
        if (raceHolder == null)
        {
            player.SetState(StateManager.Instance.flyingState);
            return;
        }
        raceUI = raceHolder.raceUI;
        checkpointForward = raceHolder.checkpointForward;
        minDotProduct = raceHolder.minDotProduct;
        timeToStart = raceHolder.timeToStart;
    }

    public override void StateUpdate()
    {
        if (!inCountdown) PreCountdown();
        else if (timeForCountdown - Time.time <= 0) FinishCountdown();
    }

    private void PreCountdown()
    {
        if (Vector3.Dot(eyeAnchor.forward, checkpointForward) > minDotProduct)
        {
            timeHolding += Time.deltaTime;
            if (timeHolding >= timeToStart)  StartCountdown();
        }
        else timeHolding = 0;
        raceUI.ChargeUp(timeHolding / timeToStart);
    }

    private void StartCountdown()
    {
        inCountdown = true;
        timeForCountdown = Time.time + raceHolder.timeForCountdown;
    }

    private void FinishCountdown()
    {
        player.SetState(StateManager.Instance.flyingState);
    }
}
