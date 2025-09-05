using UnityEngine;
using UnityEngine.XR.Management;
using UnityEngine.XR;

public class RigManager : MonoBehaviour
{
    private static RigManager _instance;
    public static RigManager instance { get { return _instance; } }

    public GameObject vrRig;
    public GameObject desktopRig;


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
        if (XRGeneralSettings.Instance.Manager.activeLoader != null)
        {
            Debug.Log("RigSwitcher: Activating VrRig");
            vrRig.SetActive(true);
            desktopRig.SetActive(false);

            usingVr = true;
            rb = vrRig.GetComponent<Rigidbody>();
            //vrRig.AddComponent<AudioListener>();
        }
        else
        {
            Debug.Log("RigSwitcher: Activating DesktopRig");
            vrRig.SetActive(false);
            desktopRig.SetActive(true);

            usingVr = false;
            rb = desktopRig.GetComponent<Rigidbody>();
            desktopRig.AddComponent<AudioListener>();
        }
    }
}