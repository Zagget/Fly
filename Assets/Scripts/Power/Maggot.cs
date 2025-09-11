using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Maggot : MonoBehaviour
{
    [Header("Tracking")]
    [SerializeField] float speed;
    [SerializeField] float visionRadius;
    [SerializeField] LayerMask mask;

    [Header("Eating")]
    [SerializeField] float eatingPower;
    [SerializeField] float eatingDistance;

    Rigidbody body;
    Food target;
    Collider targetCollider;
    bool isWalking = false;
    bool checkForFood = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (isWalking) return;
        isWalking = true;
        transform.forward = new Vector3(body.linearVelocity.x, 0, body.linearVelocity.y);
        body.linearVelocity = Vector3.zero;
        body.angularVelocity = Vector3.zero;
    }

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (isWalking) HandleCrawling();
        else HandleFlight();
    }

    private void HandleFlight()
    {
        transform.forward = body.linearVelocity;
    }

    private void HandleCrawling()
    {
        if (target == null || checkForFood)
        {
            checkForFood = true;
            FindFood();
            return;
        }

        Vector3 distance = targetCollider.ClosestPoint(transform.position) - transform.position;
        transform.forward = distance;
        Vector3 velocity = transform.forward * speed;
        velocity.y = body.linearVelocity.y;
        body.linearVelocity = velocity;

        if (distance.sqrMagnitude < Mathf.Pow(eatingDistance,2)) PowerProgression.Instance.AddEnergy(target.Eat(eatingPower * Time.deltaTime));
    }

    private void FindFood()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, visionRadius, mask, QueryTriggerInteraction.Ignore);

        float smallestDistance = Mathf.Infinity;
        float distance = 0;
        Collider closestFood = null;
        foreach (Collider collider in colliders)
        {
            if (!collider.gameObject.CompareTag("Food")) continue;
            distance = (collider.transform.position - transform.position).sqrMagnitude;
            if (distance > smallestDistance) continue;
            smallestDistance = distance;
            closestFood = collider;
        }

        targetCollider = closestFood;
        if (closestFood != null)
        {
            checkForFood = false;
            target = closestFood.transform.root.GetComponentInChildren<Food>();
        }
    }
}
