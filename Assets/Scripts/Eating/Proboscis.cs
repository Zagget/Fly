using System.Collections.Generic;
using UnityEngine;

public class Proboscis : MonoBehaviour
{
    [SerializeField] GameObject mouthAnchor;
    [SerializeField] float proboscisSpeed = 1;
    [SerializeField] float hideDistance = 1;
    [SerializeField] float distanceToEat = 1;
    [SerializeField] float EatingPower = 1;

    Dictionary<Collider, Food> foods = new();
    Vector3 targetPosition = Vector3.zero;
    Vector3 offset = default;
    bool isProboscisVisible = false;
    bool isProboscisActive = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Food")) return;
        if (!other.gameObject.TryGetComponent<Food>(out Food food)) return;
        if (foods.ContainsKey(other)) return;
        else
        {
            foods.Add(other, food);
            ShowProboscis();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Food")) return;
        RemoveFood(other);
    }

    public void RemoveFood(Collider collider)
    {
        if (!foods.ContainsKey(collider)) return;
        foods.Remove(collider);
        if (foods.Count < 1) HideProboscis();
    }

    private void Start()
    {
        if (mouthAnchor == null) gameObject.SetActive(false);
    }

    private void ShowProboscis()
    {
        if (!isProboscisVisible)
        {
            mouthAnchor.transform.localPosition = default;
            mouthAnchor.SetActive(true);
            isProboscisVisible = true;
        }
        isProboscisActive = true;
        AttachProboscis();
    }

    private void HideProboscis() { isProboscisActive = false; }

    private void AttachProboscis()
    {
        Food currentFood = null;
        Vector3 point;
        Vector3 finalPoint = default;
        float squareLength;
        float finalSquareLength = Mathf.Infinity;
        List<Collider> collidersToRemove = new ();
        foreach (var kvp in foods)
        {
            if (kvp.Key == null)
            {
                collidersToRemove.Add(kvp.Key);
                continue;
            }
            point = kvp.Key.ClosestPoint(transform.position);
            squareLength = (point - transform.position).sqrMagnitude;
            if (squareLength < finalSquareLength)
            {
                finalSquareLength = squareLength;
                finalPoint = point - transform.position;
            }
            targetPosition = finalPoint;
            currentFood = kvp.Value;
        }
        if (currentFood != null && (offset - targetPosition).sqrMagnitude < Mathf.Pow(distanceToEat, 2)) 
        {
            PowerProgression.Instance.AddEnergy(currentFood.Eat(EatingPower * Time.deltaTime, this)); 
        }

        foreach (Collider collider in collidersToRemove)
        {
            RemoveFood(collider);
        }
    }

    private void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        if (isProboscisVisible)
        {
            offset = Vector3.Lerp(offset, targetPosition, proboscisSpeed * Time.deltaTime);
            mouthAnchor.transform.position = transform.position + offset;
            if (targetPosition != default) mouthAnchor.transform.rotation = Quaternion.LookRotation(targetPosition);
        }
    }

    private void FixedUpdate()
    {
        if (isProboscisActive) AttachProboscis();
        else if (isProboscisVisible)
        {
            if (mouthAnchor.transform.localPosition.sqrMagnitude < Mathf.Pow(hideDistance, 2))
            {
                mouthAnchor.SetActive(false);
                isProboscisVisible = false;
            }
            targetPosition = default;
        }
    }
}

