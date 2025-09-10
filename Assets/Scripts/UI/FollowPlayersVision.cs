using UnityEngine;

public class FollowPlayersVision : MonoBehaviour
{
    [SerializeField] float distance;
    [SerializeField] float slerpAmount;
    
    Transform cameraTransform;
    Vector3 currentDirection = Vector3.up;

    private void Start()
    {
        cameraTransform = RigManager.instance.pTransform;
        currentDirection = cameraTransform.forward;
    }

    private void Update()
    {
        currentDirection = Vector3.Slerp(currentDirection, cameraTransform.forward, slerpAmount * Time.deltaTime);
        transform.rotation = cameraTransform.rotation;
        transform.position = cameraTransform.position + currentDirection * distance;
    }
}
