using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class DesktopMenu : MonoBehaviour
{
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject settings;

    private MenuUI menuUI;

    private VRButton[] menuButtons;
    private VRButton[] settingButtons;

    private VRButton[] currentButtons;

    private int currentSelection = 0;

    void Start()
    {
        if (RigManager.instance.usingVr)
        {
            this.enabled = false;
            return;
        }

        menuUI = GetComponent<MenuUI>();
        if (menuUI == null) Debug.LogError("menuUI is null");

        menuUI.InMenu += ChangeToMenu;
        menuUI.InSettinga += ChangeToSetting;

        menuButtons = GetAndSortButtons(menu);
        settingButtons = GetAndSortButtons(settings);
    }

    private VRButton[] GetAndSortButtons(GameObject go)
    {
        return go.GetComponentsInChildren<VRButton>()
            .OrderBy(b => b.transform.GetSiblingIndex())
            .ToArray();
    }

    public void Press(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            currentButtons[currentSelection].Press();
            currentSelection = 0;
        }
    }

    public void Navigate(int newSelection)
    {
        currentButtons[currentSelection].Hover(false);

        currentSelection += newSelection;

        if (currentSelection >= currentButtons.Length)
            currentSelection = 0;

        if (currentSelection < 0)
            currentSelection = currentButtons.Length - 1;

        currentButtons[currentSelection].Hover(true);
        Debug.Log($"Menu navigate {currentSelection}");
    }

    public void Navigate(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Vector2 input = context.ReadValue<Vector2>();

            if (input.y > 0)
            {
                Navigate(-1);
            }
            if (input.y < 0)
            {
                Navigate(1);
            }
        }
    }

    public void ChangeToMenu()
    {
        currentButtons = menuButtons;
        currentSelection = 0;

        ResetSelection();
    }

    public void ChangeToSetting()
    {
        currentButtons = settingButtons;

        ResetSelection();
    }

    private void ResetSelection()
    {
        currentSelection = 0;
        if (currentButtons.Length > 0)
        {
            foreach (var btn in currentButtons) btn.Hover(false);
            currentButtons[currentSelection].Hover(true);
        }
    }
}