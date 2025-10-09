using UnityEngine;

public class FollowPlayersVision : MonoBehaviour
{
    [SerializeField] float distance;
    [SerializeField] float slerpAmount;

    Transform cameraTransform;
    Vector3 currentDirection = Vector3.up;

    private BasePlayerState menu;
    private BasePlayerState tutorial;
    private BasePlayerState mainMenu;
    private bool followPlayer = false;

    private void Start()
    {
        cameraTransform = RigManager.instance.eyeAnchor;
        currentDirection = cameraTransform.forward;

        menu = StateManager.Instance.menuState;
        tutorial = StateManager.Instance.tutorialState;
        mainMenu = StateManager.Instance.mainMenuState;

        StateManager.Instance.OnStateChanged += CheckForMenuState;

        currentDirection = Vector3.Slerp(currentDirection, cameraTransform.forward, slerpAmount * Time.deltaTime);
        transform.rotation = cameraTransform.rotation;
        transform.position = cameraTransform.position + currentDirection * distance;
    }

    void OnDisable()
    {
        StateManager.Instance.OnStateChanged -= CheckForMenuState;
    }

    private void CheckForMenuState(BasePlayerState newState, BasePlayerState oldState)
    {
        if (newState == menu || newState == tutorial || newState == mainMenu)
        {
            followPlayer = false;
            return;
        }
        followPlayer = true;
    }

    private void Update()
    {
        if (!followPlayer) return;

        currentDirection = Vector3.Slerp(currentDirection, cameraTransform.forward, slerpAmount * Time.deltaTime);
        transform.rotation = cameraTransform.rotation;
        transform.position = cameraTransform.position + currentDirection * distance;
    }
}
