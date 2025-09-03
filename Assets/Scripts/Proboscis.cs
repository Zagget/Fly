using System.Collections.Generic;
using UnityEngine;

public class Proboscis : MonoBehaviour
{
    [SerializeField] GameObject proboscisVisuals;
    [SerializeField] float proboscisSpeed = 1;

    HashSet<Collider> foods = new();
    Vector3 targetPosition = Vector3.zero;
    bool isProboscisVisible = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Food")) return;
        if (foods.Contains(other)) return;
        else
        {
            foods.Add(other);
            if (!isProboscisVisible) ShowProboscis();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Food")) return;
        if (!foods.Contains(other)) return;
        else foods.Remove(other);
        if (foods.Count < 1) HideProboscis();
    }

    private void Awake()
    {
        if (proboscisVisuals == null) gameObject.SetActive(false);
    }

    private void ShowProboscis()
    {
        isProboscisVisible = true;
        targetPosition = default;
        proboscisVisuals.transform.localPosition = default;
        proboscisVisuals.SetActive(true);
    }

    private void HideProboscis()
    {
        isProboscisVisible = false;
        proboscisVisuals.SetActive(false);
    }

    private void AttachProboscis()
    {
        Vector3 point;
        Vector3 finalPoint;
        float squareLength;
        float finalSquareLength = Mathf.Infinity;
        foreach (Collider collider in foods)
        {
            point = collider.ClosestPoint(transform.position);
            squareLength = (point - transform.position).sqrMagnitude;
            if (squareLength < finalSquareLength)
            {
                finalSquareLength = squareLength;
                finalPoint = point - transform.position;
            }
        }
    }

    private void Update()
    {
        if (isProboscisVisible)
        {
            proboscisVisuals.transform.localPosition = Vector3.Lerp(proboscisVisuals.transform.localPosition, targetPosition, proboscisSpeed * Time.deltaTime);
            if (targetPosition != default) proboscisVisuals.transform.rotation = Quaternion.LookRotation(targetPosition);
        }
    }

    private void FixedUpdate()
    {
        if (isProboscisVisible) AttachProboscis();
    }
}

