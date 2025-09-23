using UnityEngine;
public class PlayerSound : MonoBehaviour
{
    Rigidbody rb;
    AudioSource playerSource;

    private BasePlayerState currentState;

    private HoverState hover;
    private FlyingState flying;
    private WalkingState walking;
    private MenuState menu;

    void Start()
    {
        rb = RigManager.instance.currentRb;

        hover = StateManager.Instance.hoverState;
        flying = StateManager.Instance.flyingState;
        walking = StateManager.Instance.walkingState;
        menu = StateManager.Instance.menuState;

        StateManager.Instance.OnStateChanged += OnChangedState;

        playerSource = gameObject.AddComponent<AudioSource>();

        playerSource.spatialBlend = 0f;
    }

    private void OnChangedState(BasePlayerState newState, BasePlayerState oldState)
    {
        if (newState == currentState) return;

        currentState = newState;
        Debug.Log($"Sound To {newState} From {oldState}");

        if (IsState(hover))
        {
            SoundManager.instance.PlaySound(Stopping.Random, playerSource);
        }

        if (IsState(flying))
        {
            SoundManager.instance.PlaySound(Flying.Random, playerSource, true);
        }

        if (IsState(menu) && oldState == flying)
        {
            Debug.Log("Sound ISSTATE MEnu");
            //playerSource.Stop();
            SoundManager.instance.PlaySound(Stopping.Random, playerSource);
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

    private float maxSpeed = 30;
    private void PlaySoundSpeed()
    {
        float speed = rb.linearVelocity.magnitude;

        float t = Mathf.Clamp01(speed / maxSpeed);

        playerSource.pitch = Mathf.Lerp(0.9f, 1.1f, t);
        playerSource.volume = Mathf.Lerp(0.2f, 0.8f, t);

        Debug.Log($"SOUND speed: {speed} t: {t} pitch: {playerSource.pitch} volume: {playerSource.volume} State {currentState}");
    }
}