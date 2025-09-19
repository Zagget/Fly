using UnityEngine;
using UnityEngine.InputSystem;

public class MenuUI : MonoBehaviour
{
    public GameObject menuPanel;
    public GameObject settingsPanel;

    public BoxCollider rightArm;
    public BoxCollider leftArm;
    [SerializeField] private SettingsData settings;

    private void Start()
    {
        TogglePanels(true, true);
    }

    public void ToggleMenu(InputAction.CallbackContext context)
    {
        Debug.Log("MENU");
    }

    public void EnterMenu()
    {
        TogglePanels(true, false);
    }

    public void ClickResume()
    {
        TogglePanels(false, false);
    }

    public void ClickSettings()
    {
        TogglePanels(false, true);
    }

    public void ClickPreset()
    {

    }

    public void ClickQuit()
    {

    }

    public void TogglePanels(bool menu, bool settingsMenu)
    {
        menuPanel.SetActive(menu);
        settingsPanel.SetActive(settingsMenu);
    }
}