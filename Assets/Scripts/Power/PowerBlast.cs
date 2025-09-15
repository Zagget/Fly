using UnityEngine;

[CreateAssetMenu(fileName = "new Energy Blast", menuName = "Scriptable Objects/Powers/Energy Blast")]

public class PowerBlast : BasePower
{
    [Header("Projectile")]
    [SerializeField] GameObject projectile;
    [SerializeField] AnimationCurve powerCurve;
    [SerializeField] float maxVelocity;
    [SerializeField] float maxDamage;
    [SerializeField] float maxFlightTime;
    [SerializeField] float shootOffset = 2;


    public override void Start()
    {
        GameObject instantiatedProjectile = Instantiate(projectile,
            playersRigidbody.position + playersRigidbody.transform.forward * shootOffset,
            playersRigidbody.rotation);
        if (!instantiatedProjectile.TryGetComponent<Rigidbody>(out Rigidbody rigidbody) 
            || !instantiatedProjectile.TryGetComponent<Projectile>(out Projectile projectileComponent))
        {
            Destroy(instantiatedProjectile);
            return;
        }

        float value = powerCurve.Evaluate(currentCharge/maximumCharge);
        instantiatedProjectile.transform.rotation = playersRigidbody.rotation;
        rigidbody.linearVelocity = value * maxVelocity * instantiatedProjectile.transform.forward;
        Debug.Log(value);
        projectileComponent.damage = value * maxDamage;
        Destroy(instantiatedProjectile, value * maxFlightTime);
    }
}
