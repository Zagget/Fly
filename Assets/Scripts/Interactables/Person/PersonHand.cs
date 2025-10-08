using UnityEngine;

public class PersonHand : MonoBehaviour
{
    [SerializeField] Transform personCenter;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("swatted player");
            ApplySlapForce(other.transform);
        }
    }

    void ApplySlapForce(Transform target)
    {
        Vector3 targetDir = target.position - personCenter.position;
        targetDir.y = 0f;
        Rigidbody otherRb = target.GetComponent<Rigidbody>();
        otherRb.linearVelocity = Vector3.zero;
        otherRb.AddForce(targetDir * 6000);
    }
}
