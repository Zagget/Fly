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

    [Header("Walking")]
    [SerializeField] private float distance;
    [SerializeField] private float stepDistance;

    private Rigidbody rb;
    private CapsuleCollider capCollider;
    private AudioSource loopingSource;
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

        loopingSource = gameObject.AddComponent<AudioSource>();
        miscSource = gameObject.AddComponent<AudioSource>();

        loopingSource.spatialBlend = 0f;
        miscSource.spatialBlend = 0f;
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
            {
                SoundManager.instance.PlaySound(Flying.Random, loopingSource, true);
            }
        }

        if (IsState(walking))
        {
            if (loopingSource.isPlaying)
            {
                loopingSource.Stop();
            }
        }

        if (IsState(menu) && oldState == flying)
        {
            loopingSource.volume = minVolume;
            SoundManager.instance.PlaySound(Stopping.Random, miscSource);
        }
    }

    private bool IsState(BasePlayerState state)
    {
        return currentState == state;
    }

    void Update()
    {
        if (IsState(flying) || IsState(walking))
        {
            VolumeBasedOnSpeed();
        }

        if (IsState(walking))
        {
            PlayWalkingBasedOnDistance();
        }

    }

    private void VolumeBasedOnSpeed()
    {
        float speed = rb.linearVelocity.magnitude;
        float t = Mathf.Clamp01(speed / maxSpeed);

        float targetPitch = Mathf.Lerp(minPitch, maxPitch, t);
        float targetVolume = Mathf.Lerp(minVolume, maxVolume, t);

        pitchSmooth = Mathf.Lerp(pitchSmooth, targetPitch, Time.deltaTime * smoothSpeed);
        volumeSmooth = Mathf.Lerp(volumeSmooth, targetVolume, Time.deltaTime * smoothSpeed);

        loopingSource.pitch = pitchSmooth;
        loopingSource.volume = volumeSmooth;

        //Debug.Log($"SOUND speed: {speed:F2} t: {t:F2} pitch: {pitchSmooth:F2} volume: {volumeSmooth:F2}");
    }

    private void PlayWalkingBasedOnDistance()
    {
        float frameDistance = rb.linearVelocity.magnitude * Time.deltaTime;
        distance += frameDistance;

        if (distance >= stepDistance && rb.linearVelocity.magnitude > 0.1f)
        {
            SoundManager.instance.PlaySound(Walking.Random);
            distance = 0f;
        }

        // Debug.Log($"SOUND frameDistance: {frameDistance:F2} distance: {distance:F2}");
    }

    public void PlayCollisionSound()
    {
        Debug.Log("Sound Colliding");
        if (!IsState(flying)) return;

        SoundManager.instance.PlaySound(Colliding.Random, miscSource);
        miscSource.volume = loopingSource.volume;
    }
}