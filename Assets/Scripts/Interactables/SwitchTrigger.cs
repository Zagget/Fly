using UnityEngine;

public class SwitchTrigger : MonoBehaviour
{
    LightSwitch lightSwitch;

    void Start()
    {
        lightSwitch = transform.parent.GetComponentInChildren<LightSwitch>();
    }
    void OnTriggerEnter(Collider other)
    {
        lightSwitch.ToggleLights();
    }
}
