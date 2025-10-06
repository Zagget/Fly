using UnityEngine;

public class RaceHolder : MonoBehaviour
{
    [SerializeField] Transform checkpointHolder;
    [SerializeField] int lookahead;
    Checkpoint[] checkpoints;
    int totalCheckpoints = 0;
    int currentCheckpoint = 0;

    public static RaceHolder current;
    public RaceUI raceUI;
    [HideInInspector] public Vector3 checkpointForward;
    [Range(0,1)] public float minDotProduct;
    public float timeToStart;
    public float timeForCountdown;


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

    public void CheckPointReached(int index, Collider checkpoint, Rigidbody player)
    {
        if (index == 0 && currentCheckpoint == 0) HandleStart(index, checkpoint, player);
        else if (index == totalCheckpoints - 1) HandleFinish();
        else if (index == currentCheckpoint + 1) HandleProgression();
    }

    private void HandleStart(int index, Collider checkpoint, Rigidbody player)
    {
        Vector3 relativePos = checkpoint.transform.InverseTransformPoint(player.transform.position);
        relativePos.z = 0;
        player.position = checkpoint.transform.TransformPoint(relativePos);

        current = this;
        checkpointForward = (checkpoints[index + 1].transform.position - checkpoints[index].transform.position).normalized;
        PlayerController.Instance.SetState(StateManager.Instance.racePreparationsState);
        LegRubbing.Instance.ResetRubbing();
        ShowCheckpoints();
    }

    private void HandleFinish()
    {
        currentCheckpoint = 0;
        HideCheckpoints();
    }

    private void HandleProgression()
    {
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
}
