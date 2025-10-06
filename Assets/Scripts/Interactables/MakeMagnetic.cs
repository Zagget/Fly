using UnityEngine;
public class MakeMagnetic : MonoBehaviour
{
    [SerializeField] private GameObject objectToBeDrawnTo;

    private Vector3 targetNormal;

    private Rigidbody rb;

    private bool inRange = true;

    private void Start()
    {
        if (TryGetComponent<Rigidbody>(out Rigidbody rigidBody))
        {
            rb = rigidBody;
        }
        else
        {
            enabled = false;
            Debug.Log("MakeMagnetic did not find a rigidBody " + gameObject);
            return;
        }

        Vector3 targetDirection = objectToBeDrawnTo.transform.position - transform.position;
        RaycastHit[] rayHits = Physics.RaycastAll(transform.position, targetDirection, 1000);

        foreach (RaycastHit ray in rayHits)
        {
            if (ray.collider.gameObject.name.Contains(objectToBeDrawnTo.name))
            {
                targetNormal = ray.normal;
                return;
            }
        }
    }

    private void FixedUpdate()
    {
        if (inRange)
            rb.AddForce(targetNormal * -1, ForceMode.Impulse);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains(objectToBeDrawnTo.name))
        {
            inRange = true;
            rb.constraints = RigidbodyConstraints.FreezeRotationX;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name.Contains(objectToBeDrawnTo.name))
        {
            inRange = false;
            rb.constraints = RigidbodyConstraints.None;
        }
    }
}