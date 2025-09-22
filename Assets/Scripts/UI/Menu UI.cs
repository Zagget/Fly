using System;
using UnityEngine;

public class MenuUI : MonoBehaviour
{
    public GameObject menuPanel;
    public GameObject settingsPanel;

    [SerializeField] private SettingsData settings;

    public event Action InMenu;
    public event Action InSettings;

    private ControllerData controllerData;
    private void Start()
    {
        TogglePanels(false, false);
        controllerData = GetComponent<ControllerData>();
    }

    public void EnterMenu()
    {
        TogglePanels(true, false);
        InMenu?.Invoke();
    }

    public void ClickResume()
    {
        StateManager.Instance.menuState.ExitMenu();
    }

    public void ClickSettings()
    {
        TogglePanels(false, true);
        InSettings?.Invoke();
    }

    public void ClickPreset(int preset)
    {

    }

    public void ChangeControllerMaxHeight()
    {
        controllerData.SetMaxControllerHeight();
    }

    public void SetDeadZoneSize(float size)
    {
        controllerData.SetDeadZoneSize(size);
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