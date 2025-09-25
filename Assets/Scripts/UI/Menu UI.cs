using System;
using TMPro;
using UnityEngine;

public class MenuUI : MonoBehaviour
{
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject settingsPanel;

    [SerializeField] private TextMeshProUGUI stepValue;
    [SerializeField] private TextMeshProUGUI smoothValue;

    public event Action InMenu;
    public event Action InSettings;

    [SerializeField] private SettingsData settings;
    private ControllerData controllerData;


    private void Start()
    {
        TogglePanels(false, false);
        controllerData = GetComponent<ControllerData>();
        settings.currentRotation.OnRotationChanged += UpdateUIInfo;

        settings.currentRotation.Init();
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

    public void ClickQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    }

    // Presets
    public void ClickDefault() => ClickPreset(RotationPreset.Default);
    public void ClickSnappy() => ClickPreset(RotationPreset.Snappy);
    public void ClickSmooth() => ClickPreset(RotationPreset.Smooth);
    private void ClickPreset(RotationPreset preset)
    {
        settings.currentRotation.SetPreset(preset);
    }

    // User Custom settings
    public void IncrementDegrees(bool increment) => settings.currentRotation.IncrementDegrees(increment);
    public void IncrementSmoothness(bool increment) => settings.currentRotation.IncrementSmoothness(increment);
    void UpdateUIInfo(float degree, float smoothness)
    {
        Debug.Log("index Updating UIINFO");
        smoothValue.text = $"{smoothness:0.00}";

        if (degree == 179)
            stepValue.text = $"180°";

        else
            stepValue.text = $"{degree:0}°";
    }

    // Controller
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
}