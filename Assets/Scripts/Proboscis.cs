using System.Collections.Generic;
using UnityEngine;

public class Proboscis : MonoBehaviour
{
    [SerializeField] GameObject mouthAnchor;
    [SerializeField] float proboscisSpeed = 1;
    [SerializeField] float hideDistance = 1;

    HashSet<Collider> foods = new();
    Vector3 targetPosition = Vector3.zero;
    Vector3 offset = default;
    bool isProboscisVisible = false;
    bool isProboscisActive = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Food")) return;
        if (foods.Contains(other)) return;
        else
        {
            foods.Add(other);
            ShowProboscis();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Food")) return;
        if (!foods.Contains(other)) return;
        else foods.Remove(other);
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
        Vector3 point;
        Vector3 finalPoint = default;
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
            targetPosition = finalPoint;
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
        else
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

