using UnityEngine;

public class PlaceTutorial : MonoBehaviour
{
    public Transform worldTarget;  // The world-space object to follow
    public Vector3 offset;         // Offset in world units

    void Update()
    {
        if (worldTarget == null) return;

        // Move the GameObject directly in world space
        transform.position = worldTarget.position + offset;

        Quaternion blenderFix = Quaternion.Euler(-90, 0, 0);
        transform.rotation = worldTarget.rotation * blenderFix;
    }
}