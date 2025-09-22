using System.Collections;
using UnityEngine;

public class ControllerData : MonoBehaviour
{
    private Vector3 leftControllerPosition;
    private Vector3 rightControllerPosition;

    /// <summary>
    /// Returns a Float with the size of the controller dead zone. <br></br>
    /// Needs to be used with PlayerPrefs.
    /// </summary> 
    public static readonly string deadZoneSizeKey;
    /// <summary>
    /// Returns a Float which is the max height the player set they can reach. <br></br>
    /// Needs to be used with PlayerPrefs.
    /// </summary>
    public static readonly string maxControllerHeightKey;

    private float deadZoneCalibrationTime = 2;

    /// <summary>
    /// Sets the max controller height to the average of the 2 controllers position.
    /// </summary>
    public void SetMaxControllerHeight()
    {

        if (!RigManager.instance.usingVr)
        {
            Debug.LogWarning("Not in VR, cant set Max Controller Height " + name);
            return;
        }

        StartCoroutine(nameof(SetControllerHeight));
    }

    IEnumerator SetControllerHeight()
    {
        float timeTillSet = 2;
        yield return new WaitForSeconds(timeTillSet);

        leftControllerPosition = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LHand);
        rightControllerPosition = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RHand);



        float maxControllerHeight = (leftControllerPosition.y + rightControllerPosition.y) / 2;
        PlayerPrefs.SetFloat(maxControllerHeightKey, maxControllerHeight);
        Debug.Log("Controller height set to " + maxControllerHeight);
    }

    /// <summary>
    /// Used if the player only wants to use 1 controller to set max height.
    /// </summary>
    /// <param name="controllerType"></param>
    public void SetIndividualMaxControllerHeight(OVRInput.Controller controllerType)
    {
        if (!RigManager.instance.usingVr)
        {
            Debug.LogWarning("Not in VR, cant set individual Max Controller Height " + name);
            return;
        }

        float maxControllerHeight = OVRInput.GetLocalControllerPosition(controllerType).y;
        PlayerPrefs.SetFloat(maxControllerHeightKey, maxControllerHeight);
    }

    public void SetDeadZoneSize(float size)
    {
        PlayerPrefs.SetFloat(deadZoneSizeKey, size);
    }
}