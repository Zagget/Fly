using UnityEngine;

[CreateAssetMenu(fileName = "new Energy Blast", menuName = "Scriptable Objects/Powers/Energy Blast")]

public class PowerBlast : BasePower
{
    public override void Start()
    {
        Debug.Log("Start");
    }

    public override void Update()
    {
        Debug.Log("Update");
    }

    public override void End()
    {
        Debug.Log("End");
    }
}
