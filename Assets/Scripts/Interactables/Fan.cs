using UnityEngine;

public class Fan : MonoBehaviour
{
    [Header("Fan settings")]
    [Range(0, 100)][SerializeField] float blowForce = 10;

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.TryGetComponent<Rigidbody>(out Rigidbody otherRB) && !otherRB.isKinematic)
        {
            Vector3 forceDir = transform.forward * blowForce;
            otherRB.AddForce(forceDir);
        }
    }
}
