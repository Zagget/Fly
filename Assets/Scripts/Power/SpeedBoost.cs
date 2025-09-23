using UnityEngine;

public class SpeedBoost : BasePower
{
    public override void Continues()
    {
        throw new System.NotImplementedException();
    }

    //public override void Start()
    //{
        
    //    currentCharge = Mathf.Clamp(currentCharge, 0f, maximumCharge);
    //    float boostSpeed = 20f * (currentCharge / maximumCharge);
    //    playersRigidbody.AddForce(playersRigidbody.transform.forward * boostSpeed, ForceMode.VelocityChange);
    //    Debug.Log($"Speed Boost activated with currentCharge: {currentCharge}, speed: {boostSpeed}");
    //}
}
