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
        StateManager.Instance.OnStateChanged += OnStateChanged;
        playerController.SetState(StateManager.Instance.mainMenuState);
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
        StateManager.Instance.OnStateChanged -= OnStateChanged;
        playerController.SetState(StateManager.Instance.flyingState);
        gameObject.SetActive(false);
    }

    public void PressSettings()
    {
        gameObject.SetActive(false);
        playerController.SetState(StateManager.Instance.menuState);
    }

    public void PressCredits()
    {
        foreach (GameObject go in UIToDisableWithCredits)
        {
            go.SetActive(!go.activeSelf);
        }

        UIToEnableWithCredits.SetActive(!UIToEnableWithCredits.activeSelf);
    }

    public void PressReturn()
    {
        UIToEnableWithCredits.SetActive(!UIToEnableWithCredits.activeSelf);
        foreach (GameObject go in UIToDisableWithCredits)
        {
            go.SetActive(!go.activeSelf);
        }
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