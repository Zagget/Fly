using UnityEditor;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject[] UIToDisableWithCredits;
    [SerializeField] private GameObject UIToEnableWithCredits;
    [SerializeField] private MenuUI menuUI;

    private PlayerController playerController;

    private void Start()
    {
        playerController = PlayerController.Instance;
        // playerController.SetState(StateManager.Instance.mainMenuState);
        StateManager.Instance.OnStateChanged += OnStateChanged;
    }

    private void OnStateChanged(BasePlayerState newState, BasePlayerState lastState)
    {
        if (newState == StateManager.Instance.mainMenuState)
        {
            gameObject.SetActive(true);
        }
    }

    public void PressPlay()
    {
        playerController.SetState(StateManager.Instance.flyingState);
        StateManager.Instance.OnStateChanged -= OnStateChanged;
        gameObject.SetActive(false);
    }

    public void PressSettings()
    {
        gameObject.SetActive(false);
        menuUI.EnterMenu();
    }

    public void PressCredits()
    {
        foreach (GameObject go in UIToDisableWithCredits)
        {
            go.SetActive(!go.activeSelf);
        }

        UIToEnableWithCredits.SetActive(!UIToEnableWithCredits.activeSelf);
    }

    public void PressQuit()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}