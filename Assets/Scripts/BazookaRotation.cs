using UnityEngine;
public class BazookaRotation : MonoBehaviour
{
    Rigidbody playerRB;
    Transform playerTransform;
    [SerializeField] private float rotationSpeed;
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

        Vector3 _direction = (target.position - transform.position).normalized;
        Quaternion _lookRotation = Quaternion.LookRotation(_direction * -1);

        transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * rotationSpeed);
    }
}