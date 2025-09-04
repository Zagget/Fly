using UnityEngine;
using UnityEngine.XR;

public class RigSwitcher : MonoBehaviour
{
    public GameObject vrRig;
    public GameObject desktopRig;

    void Start()
    {
        // Check if XR is initialized and a headset is connected
        if (XRSettings.isDeviceActive)
        {
            Debug.Log("RigSwitcher: Activating VrRig");
            vrRig.SetActive(true);
            desktopRig.SetActive(false);
        }
        else
        {
            Debug.Log("RigSwitcher: Activating DesktopRig");
            vrRig.SetActive(false);
            desktopRig.SetActive(true);
        }
    }
}