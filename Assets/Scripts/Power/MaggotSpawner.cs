using UnityEngine;


[CreateAssetMenu(fileName = "new magot spawner", menuName = "Scriptable Objects/Powers/Magot spawner")]
public class MaggotSpawner : BasePower
{
    [Header("Spawning")]
    [SerializeField] GameObject maggotPrefab;
    [SerializeField] AnimationCurve maggotCurve;
    [SerializeField] int maxMaggots;
    [SerializeField] float speed;
    [SerializeField] float spawnOffset;
    [SerializeField, Range(0, 1)] float varianceAmount;

    public override void Start()
    {
        float value = maggotCurve.Evaluate(currentCharge / maximumCharge);
        int maggotAmount = Mathf.RoundToInt(value * maxMaggots);
        Vector3 centerVector = Vector3.Slerp(rigManager.currentRb.transform.forward, rigManager.currentRb.transform.up, 0.5f);
        Vector2 randomOffset;
        Vector3 launchDirection;
        GameObject maggot;
        float variance;
        for (int i = 0; i < maggotAmount; i++)
        {
            variance = Random.Range(1 - varianceAmount, 1 + varianceAmount);
            randomOffset = Random.insideUnitCircle * 0.5f;
            launchDirection = new Vector3(randomOffset.x, 0, randomOffset.y) + centerVector;
            maggot = Instantiate(maggotPrefab, rigManager.currentRb.transform.position + launchDirection * spawnOffset, Quaternion.LookRotation(launchDirection));
            if (maggot.TryGetComponent<Rigidbody>(out Rigidbody rigidbody))
            {
                rigidbody.linearVelocity = variance * speed * launchDirection;
                Destroy(maggot, duration);
            }
            else Destroy(maggot);
        }
    }
}
