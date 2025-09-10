using UnityEngine;
using UnityEngine.XR.Management;

public class RigManager : MonoBehaviour
{
    private static RigManager _instance;
    public static RigManager instance { get { return _instance; } }

    [Header("Refs")]
    [SerializeField] private GameObject vrRig;
    [SerializeField] private GameObject desktopRig;


    [Header("Shared References")]
    public bool usingVr;
    public Rigidbody currentRb;
    public Transform pTransform;
    public Transform eyeAnchor;
    public Camera desktopCamera;

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
        if (XRGeneralSettings.Instance.Manager.activeLoader != null)
        {
            Debug.Log("RigSwitcher: Activating VrRig");

            SetRigsActive(true, false);

            currentRb = vrRig.GetComponent<Rigidbody>();
            pTransform = vrRig.transform;
            eyeAnchor = pTransform.GetComponentInChildren<AudioListener>().transform;
        }
        else
        {
            Debug.Log("RigSwitcher: Activating DesktopRig");

            SetRigsActive(false, true);

            currentRb = desktopRig.GetComponent<Rigidbody>();
            desktopCamera = desktopRig.GetComponent<Camera>();
            pTransform = desktopCamera.transform;
            eyeAnchor = desktopCamera.transform;
        }
    }

    private void SetRigsActive(bool useVrRig, bool useDesktopRig)
    {
        vrRig.SetActive(useVrRig);
        desktopRig.SetActive(useDesktopRig);

        usingVr = useVrRig;
    }
}