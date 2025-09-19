using System.Collections;
using UnityEngine;

public class ControllerData : MonoBehaviour
{
    private Vector3 leftControllerPosition;
    private Vector3 rightControllerPosition;

    /// <summary>
    /// The center will be returned as a String, needs to be converted to a Vector3. <br></br>
    /// Needs to be used with PlayerPrefs.
    /// </summary>
    public static readonly string deadZoneCenterKey;

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
        leftControllerPosition = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LHand);
        rightControllerPosition = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RHand);

        float maxControllerHeight = (leftControllerPosition.y + rightControllerPosition.y) / 2;
        PlayerPrefs.SetFloat(maxControllerHeightKey, maxControllerHeight);
    }

    /// <summary>
    /// Used if the player only wants to use 1 controller to set max height.
    /// </summary>
    /// <param name="controllerType"></param>
    public void SetIndividualMaxControllerHeight(OVRInput.Controller controllerType)
    {
        float maxControllerHeight = OVRInput.GetLocalControllerPosition(controllerType).y;
        PlayerPrefs.SetFloat(maxControllerHeightKey, maxControllerHeight);
    }


    public void SetDeadZoneSize(float size)
    {
        PlayerPrefs.SetFloat(deadZoneSizeKey, size);
    }

    public void SetDeadZoneCenter()
    {
        StartCoroutine(nameof(DeadZoneCalibrator));
    }
    IEnumerator DeadZoneCalibrator()
    {
        float timer = 0;
        Vector3 deadZoneAverages = new();
        int amountOfChecks = 0;

        while (timer < deadZoneCalibrationTime)
        {
            leftControllerPosition = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LHand);
            rightControllerPosition = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RHand);

            deadZoneAverages += (leftControllerPosition + rightControllerPosition) / 2;
            amountOfChecks++;
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        deadZoneAverages = deadZoneAverages / amountOfChecks;
        PlayerPrefs.SetString(deadZoneCenterKey, deadZoneAverages.ToString());
    }
}