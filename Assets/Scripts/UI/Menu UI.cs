using UnityEngine;
using UnityEngine.InputSystem;

public class MenuUI : MonoBehaviour
{
    public GameObject menuPanel;
    public GameObject settingsPanel;

    [SerializeField] private SettingsData settings;

    bool inMenu = false;

    private void Start()
    {
        TogglePanels(false, false);
    }

    public void ToggleMenu(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            inMenu = !inMenu;

            if (inMenu)
            {
                TogglePanels(true, false);
            }
            else
            {
                TogglePanels(false, false);
            }

        }
    }

    public void EnterMenu()
    {
        TogglePanels(true, false);
    }

    public void ClickResume()
    {
        inMenu = false;
        TogglePanels(false, false);
    }

    public void ClickSettings()
    {
        TogglePanels(false, true);
    }

    public void ClickPreset(int preset)
    {

    }

    public void TogglePanels(bool menu, bool settingsMenu)
    {
        menuPanel.SetActive(menu);
        settingsPanel.SetActive(settingsMenu);
    }

    public void ClickQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    }

}