using UnityEngine;
using UnityEngine.Audio;

public class PlayerSound : MonoBehaviour
{
    Rigidbody rb;
    AudioSource playerSource;

    void Start()
    {
        rb = RigManager.instance.currentRb;
        StateManager.Instance.OnStateChanged += OnChangedState;

        playerSource = gameObject.AddComponent<AudioSource>();

        playerSource.spatialBlend = 0;
    }

    private void OnChangedState(BasePlayerState newState, BasePlayerState oldState)
    {
        SoundManager.instance.PlaySound(Flying.Random, playerSource);
    }
}