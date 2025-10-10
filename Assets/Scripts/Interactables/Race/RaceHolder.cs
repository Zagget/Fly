using UnityEngine;

public class RaceHolder : MonoBehaviour
{

    [SerializeField] RaceUI raceUI;
    [SerializeField] Transform checkpointHolder;
    [SerializeField] int lookahead;
    [SerializeField,Range(0,1)] float minDotProduct;
    [SerializeField] float timeToStart;
    [SerializeField] float timeForCountdown;
    [SerializeField] float raceTime;
    Checkpoint[] checkpoints;
    int totalCheckpoints = 0;
    int currentCheckpoint = 0;

    states currentState;
    Transform player;
    Vector3 checkpointForward;
    float timeInStandby;


    private void Awake()
    {
        checkpoints = checkpointHolder.GetComponentsInChildren<Checkpoint>(true);
        totalCheckpoints = checkpoints.Length;
        foreach (Checkpoint checkPoint in checkpoints)
        {
            checkPoint.holder = this;
        }
        HideCheckpoints();
    }

    private void Update()
    {
        if (currentState == states.standby) { StandbyCheck(); }
    }

    public void CheckPointReached(int index, Transform checkpoint, Rigidbody player)
    {
        if (index == 0 && currentCheckpoint == 0 && currentState == states.idle) HandleStart(index, checkpoint, player);
        else if (index == totalCheckpoints - 1) HandleFinish();
        else if (index == currentCheckpoint + 1) HandleProgression();
    }

    private void HandleStart(int index, Transform checkpoint, Rigidbody player)
    {
        Vector3 relativePos = checkpoint.InverseTransformPoint(player.transform.position);
        relativePos.z = 0;
        player.position = checkpoint.transform.TransformPoint(relativePos);
        this.player = RigManager.instance.eyeAnchor; 
        checkpointForward = (checkpoints[index + 1].transform.position - checkpoints[index].transform.position).normalized;
        currentState = states.standby;
        timeInStandby = 0;

        RaceHUD.Instance.StartStandby();
        PlayerController.Instance.SetState(StateManager.Instance.racePreparationsState);
        RigManager.instance.legRubbing.ResetRubbing();
        ShowCheckpoints();
    }

    private void HandleFinish()
    {
        SoundManager.instance.PlaySound(RaceSounds.FinishLine);
        RaceHUD.Instance.ExitRace();
        ExitRace();
    }

    private void HandleProgression()
    {
        SoundManager.instance.PlaySound(RaceSounds.okay);
        currentCheckpoint++;
        ShowCheckpoints();
    }

    private void HideCheckpoints()
    {
        for (int i = 1; i < totalCheckpoints; i++)
        {
            checkpoints[i].Hide();
        }
    }

    private void ShowCheckpoints()
    {
        for (int i = currentCheckpoint; i < currentCheckpoint + lookahead + 1; i++)
        {
            if (i < totalCheckpoints) checkpoints[i].Show();
            else break;
        }
    }

    private void StandbyCheck()
    {
        if (Vector3.Dot(player.forward, checkpointForward) > minDotProduct)
        {
            timeInStandby += Time.deltaTime;
            if (timeInStandby >= timeToStart) StartCountdown();
        }
        else timeInStandby = 0;
        raceUI.ChargeUp(timeInStandby / timeToStart);
    }

    private void StartCountdown()
    {
        SoundManager.instance.PlaySound(RaceSounds.raceCountdown);
        RigManager.instance.legRubbing.gameObject.SetActive(true);
        RaceHUD.Instance.StartCountdown(raceTime, this);
        currentState = states.countdown;
        raceUI.StartCountDown(timeForCountdown);
        Invoke(nameof(StartRace), timeForCountdown);
    }

    public void StartRace()
    {
        RigManager.instance.legRubbing.gameObject.SetActive(false);
        RaceHUD.Instance.StartRace();
        PlayerController.Instance.SetState(StateManager.Instance.racingState);
        currentState = states.racing;
    }

    public void ExitRace()
    {
        PlayerController.Instance.SetState(StateManager.Instance.flyingState);
        currentState = states.idle;
        currentCheckpoint = 0;
        HideCheckpoints();
    }

    public enum states
    {
        idle,
        standby,
        countdown,
        racing
    }
}
