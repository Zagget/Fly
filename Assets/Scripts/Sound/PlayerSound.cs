using System.Collections;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    [Header("Smooth Settings")]
    [SerializeField] private float maxSpeed = 30;
    [SerializeField] private float volumeSmooth = 0.2f;
    [SerializeField] private float smoothSpeed = 10f;

    [Header("Volume")]
    [SerializeField] private float minVolume = 0.2f;
    [SerializeField] private float maxVolume = 0.7f;

    private AudioSource loopingSource;
    private AudioSource miscSource;

    private BasePlayerState currentState;
    private HoverState hover;
    private FlyingState flying;
    private MenuState menu;

    private Rigidbody rb;

    void Awake()
    {
        loopingSource = gameObject.AddComponent<AudioSource>();
        miscSource = gameObject.AddComponent<AudioSource>();

        loopingSource.spatialBlend = 0f;
        miscSource.spatialBlend = 0f;
    }

    void Start()
    {
        rb = RigManager.instance.currentRb;

        hover = StateManager.Instance.hoverState;
        flying = StateManager.Instance.flyingState;
        menu = StateManager.Instance.menuState;

        StateManager.Instance.OnStateChanged += OnChangedState;
    }

    private void OnChangedState(BasePlayerState newState, BasePlayerState oldState)
    {
        if (newState == currentState) return;

        currentState = newState;
        //Debug.Log($"Sound To {newState} From {oldState}");

        if (IsState(hover))
        {
            SoundManager.instance.PlaySound(Stopping.Random, miscSource);
        }

        if (IsState(flying))
        {
            if (!loopingSource.isPlaying)
                SoundManager.instance.PlaySound(Flying.Random, loopingSource, true);
        }

        if (IsState(menu) && oldState == flying)
        {
            miscSource.volume = minVolume;
            SoundManager.instance.PlaySound(Stopping.Random, miscSource);
        }
    }

    private bool IsState(BasePlayerState state)
    {
        return currentState == state;
    }

    private void Update()
    {
        if (IsState(flying))
        {
            VolumeBasedOnSpeed();
        }
    }

    private void VolumeBasedOnSpeed()
    {
        float speed = rb.linearVelocity.magnitude;
        float t = Mathf.Clamp01(speed / maxSpeed);

        float targetVolume = Mathf.Lerp(minVolume, maxVolume, t);

        volumeSmooth = Mathf.Lerp(volumeSmooth, targetVolume, Time.deltaTime * smoothSpeed);

        loopingSource.volume = volumeSmooth;

        //Debug.Log($"SOUND speed: {speed:F2} t: {t:F2} pitch: {pitchSmooth:F2} volume: {volumeSmooth:F2}");
    }

    public void PlayCollisionSound()
    {
        //Debug.Log("Sound Colliding");
        if (!IsState(flying)) return;

        SoundManager.instance.PlaySound(Colliding.Random, miscSource);
        miscSource.volume = loopingSource.volume;
    }
}