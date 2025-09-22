using UnityEngine;

public class FollowPlayersVision : MonoBehaviour
{
    [SerializeField] float distance;
    [SerializeField] float slerpAmount;

    Transform cameraTransform;
    Vector3 currentDirection = Vector3.up;

    private BasePlayerState menu;
    private bool followPlayer = true;

    private void Start()
    {
        cameraTransform = RigManager.instance.eyeAnchor;
        currentDirection = cameraTransform.forward;

        menu = StateManager.Instance.menuState;

        StateManager.Instance.OnStateChanged += CheckForMenuState;
    }

    void OnDisable()
    {
        StateManager.Instance.OnStateChanged -= CheckForMenuState;
    }

    private void CheckForMenuState(BasePlayerState newState, BasePlayerState oldState)
    {
        if (newState == menu)
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
