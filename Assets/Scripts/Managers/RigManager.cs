using UnityEngine;
using UnityEngine.XR;

public class RigManager : MonoBehaviour
{
    private static RigManager _instance;
    public static RigManager instance { get { return _instance; } }

    public GameObject vrRig;
    public GameObject desktopRig;

    // Just for easy reference to rigidbody
    public GameObject trackingSpace;

    public bool usingVr;
    public Rigidbody rb;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        CheckRig();
    }

    private void CheckRig()
    {
        // Check if XR is initialized and a headset is connected
        if (XRSettings.isDeviceActive)
        {
            Debug.Log("RigSwitcher: Activating VrRig");
            vrRig.SetActive(true);
            desktopRig.SetActive(false);

            usingVr = true;
            rb = trackingSpace.GetComponent<Rigidbody>();
        }
        else
        {
            Debug.Log("RigSwitcher: Activating DesktopRig");
            vrRig.SetActive(false);
            desktopRig.SetActive(true);

            usingVr = false;
            rb = desktopRig.GetComponent<Rigidbody>();
        }
    }
}