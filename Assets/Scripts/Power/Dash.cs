using UnityEngine;

[CreateAssetMenu(fileName = "new Dash", menuName = "Scriptable Objects/Powers/Dash Power")]
public class DashPower : BasePower
{
    [SerializeField] float speedFactor = 10;
    public override void Start()
    {
        playersRigidbody.linearVelocity = Vector3.zero;

        currentCharge = Mathf.Clamp(currentCharge, 0f, maximumCharge);

        float dashSpeed = speedFactor * (currentCharge / maximumCharge);

        playersRigidbody.AddForce(playersRigidbody.transform.forward * dashSpeed, ForceMode.VelocityChange);

        Debug.Log($"Dashed forward with, currentCharge: {currentCharge}, speed: {dashSpeed}");
    }
}