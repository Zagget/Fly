using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    [Header("Smooth Settings")]
    [SerializeField] private float maxSpeed = 30;
    [SerializeField] private float pitchSmooth = 0.9f;
    [SerializeField] private float volumeSmooth = 0.2f;
    [SerializeField] private float smoothSpeed = 10f;

    [Header("Volume/Pitch")]
    [SerializeField] private float minVolume = 0.2f;
    [SerializeField] private float maxVolume = 0.7f;
    [SerializeField] private float minPitch = 0.9f;
    [SerializeField] private float maxPitch = 1.1f;

    private Rigidbody rb;
    private CapsuleCollider capCollider;
    private AudioSource buzzingSource;
    private AudioSource miscSource;

    private BasePlayerState currentState;
    private HoverState hover;
    private FlyingState flying;
    private WalkingState walking;
    private MenuState menu;

    void Start()
    {
        rb = RigManager.instance.currentRb;
        capCollider = RigManager.instance.currentCollider;

        hover = StateManager.Instance.hoverState;
        flying = StateManager.Instance.flyingState;
        walking = StateManager.Instance.walkingState;
        menu = StateManager.Instance.menuState;

        StateManager.Instance.OnStateChanged += OnChangedState;

        buzzingSource = gameObject.AddComponent<AudioSource>();
        miscSource = gameObject.AddComponent<AudioSource>();

        buzzingSource.spatialBlend = 0f;
        miscSource.spatialBlend = 0f;
    }

    private void OnChangedState(BasePlayerState newState, BasePlayerState oldState)
    {
        if (newState == currentState) return;

        currentState = newState;
        Debug.Log($"Sound To {newState} From {oldState}");

        if (IsState(hover))
        {
            SoundManager.instance.PlaySound(Stopping.Random, miscSource);
        }

        if (IsState(flying))
        {
            if (!buzzingSource.isPlaying)
            {
                SoundManager.instance.PlaySound(Flying.Random, buzzingSource, true);
            }
        }

        if (IsState(menu) && oldState == flying)
        {
            buzzingSource.volume = minVolume;
            SoundManager.instance.PlaySound(Stopping.Random, miscSource);
        }
    }

    private bool IsState(BasePlayerState state)
    {
        return currentState == state;
    }

    void Update()
    {
        if (IsState(flying))
        {
            PlaySoundSpeed();
        }
    }

    private void PlaySoundSpeed()
    {
        float speed = rb.linearVelocity.magnitude;
        float t = Mathf.Clamp01(speed / maxSpeed);

        float targetPitch = Mathf.Lerp(minPitch, maxPitch, t);
        float targetVolume = Mathf.Lerp(minVolume, maxVolume, t);

        pitchSmooth = Mathf.Lerp(pitchSmooth, targetPitch, Time.deltaTime * smoothSpeed);
        volumeSmooth = Mathf.Lerp(volumeSmooth, targetVolume, Time.deltaTime * smoothSpeed);

        buzzingSource.pitch = pitchSmooth;
        buzzingSource.volume = volumeSmooth;

        //Debug.Log($"SOUND speed: {speed:F2} t: {t:F2} pitch: {pitchSmooth:F2} volume: {volumeSmooth:F2}");
    }

    public void PlayCollisionSound()
    {
        Debug.Log("Sound Colliding");
        if (!IsState(flying)) return;

        SoundManager.instance.PlaySound(Colliding.Random, miscSource);
        miscSource.volume = buzzingSource.volume;
    }
}