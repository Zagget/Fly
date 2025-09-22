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

    public void ExitMenu()
    {
        inMenu = false;
        TogglePanels(false, false);

        StateManager.Instance.menuState.ExitMenu();
    }

    public void GoBackToMenu()
    {
        TogglePanels(true, false);
    }

    public void ClickSettings()
    {
        TogglePanels(false, true);
    }

    public void ClickPreset(int preset)
    {

    }

    public void ChangeControllerMaxHeight()
    {
        Debug.Log("Change Height Button Pressed");
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