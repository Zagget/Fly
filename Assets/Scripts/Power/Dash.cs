using UnityEngine;

public class DashPower : BasePower
{
    public DashPower(float speed, float damage, float cooldown)
        : base(speed, damage, cooldown) { }

    public override void Activate(Rigidbody player, float currentCharge)
    {
        if (player == null) return;

        player.linearVelocity = Vector3.zero;

        currentCharge = Mathf.Clamp(currentCharge, 0f, maximumCharge);

        float dashSpeed = baseDashSpeed * (currentCharge / maximumCharge);

        player.AddForce(player.transform.forward * dashSpeed, ForceMode.VelocityChange);

        Debug.Log($"Dashed forward with, currentCharge: {currentCharge}, speed: {dashSpeed}");
    }
}