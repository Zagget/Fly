using UnityEngine;
public class BazookaRotation : MonoBehaviour
{
    Rigidbody playerRB;
    Transform playerTransform;
    [SerializeField] float rotationDelta;
    void Start()
    {
        playerRB = RigManager.instance.currentRb;
        playerTransform = playerRB.transform;
    }

    void Update()
    {
        VerticalRotation(playerTransform);
    }

    private void VerticalRotation(Transform target)
    {
        Vector3 targetPos = new Vector3(transform.position.x, target.position.y, transform.position.z);

        transform.rotation = Quaternion.RotateTowards(transform.rotation,target.rotation, rotationDelta * Time.deltaTime);
    }
}