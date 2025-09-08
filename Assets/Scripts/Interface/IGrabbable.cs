using Oculus.Interaction.Samples;
using UnityEngine;

public interface IGrabbable
{
    Rigidbody rb { get; }
    void OnGrab(Transform hand);
    void OnRelease(Vector3 velocity, Vector3 rotationSpeed);
}
