using UnityEngine;

public class CollisionSoundPlayer : MonoBehaviour
{
    private PlayerSound ps;

    private void Start()
    {
        ps = FindFirstObjectByType<PlayerSound>();
    }

    void OnCollisionEnter(Collision collision)
    {
        ps.PlayCollisionSound();
    }
}
