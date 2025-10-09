using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Collections;
using UnityEngine.InputSystem;
using System;

public class SoundManager : MonoBehaviour
{
    private static SoundManager _instance;
    public static SoundManager instance { get { return _instance; } }

    [SerializeField] public AudioMixer audioMixer;
    [SerializeField] public AudioMixerVolumeData mixerData;
    [SerializeField, Range(1, 100)] private int _amountOfAudioSources = 40;
    [SerializeField, Range(1, 100)] private int _amountOfAudioSourcesLoop = 5;

    public event Action SoundDoneLoading;
    private List<SoundData> _currentSoundData = new List<SoundData>();
    private AudioSourcePool _oneShotPool2D;
    private AudioSourcePool _loopPool2D;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);

            var handle = Addressables.LoadAssetsAsync<SoundData>("Sound");
            handle.Completed += OnSoundsLoaded;
            CreateAudioSources();
        }
        else
        {
            Debug.LogWarning($"{name} Duplicate SoundManager destroyed.");
            Destroy(gameObject);
        }
    }

    // void Update()
    // {
    //     if (Keyboard.current.spaceKey.wasPressedThisFrame)
    //     {
    //         PlaySound(Test123.CoinDropping);
    //     }
    // }

    private void OnSoundsLoaded(AsyncOperationHandle<IList<SoundData>> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            Debug.Log($"{name} All SoundData loaded successfully!");
            _currentSoundData = new List<SoundData>(handle.Result);
            mixerData.ApplyAllVolume(audioMixer);
            SoundDoneLoading?.Invoke();
        }
        else
        {
            Debug.LogError($"{name} Failed to load SoundData assets.");
        }
    }

    /// <summary>
    /// Creates audio sources for 2D sounds, plays at a constant volume, unaffected by distance or listener position.
    /// Used for Player sounds, UI clicks, music etc.
    /// </summary>
    private void CreateAudioSources()
    {
        GameObject audioSourceContainerOneShot = new GameObject("AudioSourcesOneShot");
        GameObject audioSourceContainerLoops = new GameObject("AudioSourcesLoop");

        audioSourceContainerOneShot.transform.SetParent(transform);
        audioSourceContainerLoops.transform.SetParent(transform);

        _oneShotPool2D = new AudioSourcePool(audioSourceContainerOneShot, _amountOfAudioSources);
        _loopPool2D = new AudioSourcePool(audioSourceContainerLoops, _amountOfAudioSourcesLoop);
    }

    /// <summary>
    /// Plays a sound identified by the given enum value.
    /// </summary>
    /// <typeparam name="T">An enum type representing the sound identifier.</typeparam>
    /// <param name="sound">The enum value specifying which sound to play.</param>
    /// <param name="source"> For 3D Sounds, use a specific audio soruce.</param>
    /// <param name="loop">If true, the sound will loop continuously; otherwise, it will play once.</param>
    public void PlaySound<T>(T sound, AudioSource source = null, bool loop = false) where T : struct, System.Enum
    {
        var sid = SoundHelper.GetSoundIdentifiers(sound);

        SoundData soundData = SoundHelper.GetSoundData(_currentSoundData, sid.dataName);

        if (!SoundHelper.IsSoundDataValid(soundData, sid.dataName, this.name)) return;


        if (sid.songName == "Random")
        {
            SoundData.SoundEntry randomEntry = soundData.sounds[UnityEngine.Random.Range(0, soundData.sounds.Length)];
            PlayClip(randomEntry, source, loop);
            return;
        }

        SoundData.SoundEntry entry = SoundHelper.GetSound(soundData, sid.songName);

        if (entry == null)
        {
            //  Debug.Log($"{name} {sid.songName} not found in {sid.dataName}");
            return;
        }

        PlayClip(entry, source, loop);
    }

    private void PlayClip(SoundData.SoundEntry entry, AudioSource source, bool loop)
    {
        //        Debug.Log($"{name} in playclip");

        if (source == null)
        {
            if (loop)
            {
                source = _loopPool2D.GetAvailable();
                _loopPool2D.AddInLookUp(source, entry.name);
                //  Debug.Log($"{name} {entry.name} added to loop");
            }
            else
            {
                source = _oneShotPool2D.GetAvailable();
            }
        }

        source.loop = loop;
        source.clip = entry.clip;
        source.volume = entry.volume;

        float randomPitch = UnityEngine.Random.Range(entry.minPitch, entry.maxPitch);
        source.pitch = randomPitch;

        source.outputAudioMixerGroup = entry.mixer;
        source.Play();

        // Debug.Log($"{name} Played {entry.name} on {entry.mixer} on AudioSource: {source} looping: {loop}");
    }

    /// <summary>
    /// Stops a looping sound identified by the given enum value, fading it out over the specified duration.
    /// </summary>
    /// <typeparam name="T">An enum type representing the sound identifier.</typeparam>
    /// <param name="song">The enum value specifying which looping sound to stop.</param>
    /// <param name="fadeOutDuration">The duration in seconds over which to fade out the sound. Default is 2 seconds.</param>
    public void StopSound<T>(T song, float fadeOutDuration = 2f) where T : struct, System.Enum
    {
        var sid = SoundHelper.GetSoundIdentifiers(song);

        AudioSource sound = _loopPool2D.GetSourceBySoundName(sid.songName);
        if (sound == null)
        {
            Debug.Log($"{name} Couldn't find looping source for: {sid.songName}");
            return;
        }

        StartCoroutine(FadeOut(sound, fadeOutDuration));
    }

    /// <summary>
    /// Crossfades between two looping sounds identified by the given enum values over the specified duration.
    /// </summary>
    /// <typeparam name="T">An enum type representing the sound identifiers.</typeparam>
    /// <param name="from">The enum value specifying the sound to fade out.</param>
    /// <param name="to">The enum value specifying the sound to fade in.</param>
    /// <param name="crossFadeDuration">The duration in seconds for the crossfade. Default is 3 seconds.</param>
    public void CrossFade<T>(T from, T to, float crossFadeDuration = 3f) where T : struct, System.Enum
    {
        var fromSid = SoundHelper.GetSoundIdentifiers(from);
        var toSid = SoundHelper.GetSoundIdentifiers(to);

        SoundData fromData = SoundHelper.GetSoundData(_currentSoundData, fromSid.dataName);
        SoundData toData = SoundHelper.GetSoundData(_currentSoundData, toSid.dataName);

        if (!SoundHelper.IsSoundDataValid(fromData, fromSid.dataName, name) ||
            !SoundHelper.IsSoundDataValid(toData, toSid.dataName, name))
            return;

        var fromEntry = SoundHelper.GetSound(fromData, fromSid.songName);
        var toEntry = SoundHelper.GetSound(toData, toSid.songName);

        if (fromEntry == null || toEntry == null)
        {
            Debug.Log($"{name} Could not find from/to entries.");
            return;
        }

        // Find the playing source for the 'from' clip
        AudioSource fromSource = _loopPool2D.GetSourceBySoundName(fromEntry.name);
        if (fromSource == null)
        {
            Debug.Log($"{name} No active source for {fromEntry.name}, playing {toEntry.name} immediately.");
            // PlayClip(toEntry, loop: true);
            return;
        }

        // Create new source for 'to' clip
        AudioSource toSource = _loopPool2D.GetAvailable();
        toSource.clip = toEntry.clip;
        toSource.loop = true;
        toSource.outputAudioMixerGroup = toEntry.mixer;
        toSource.volume = 0f;
        _loopPool2D.AddInLookUp(toSource, toEntry.name);
        toSource.Play();

        // Start crossfade
        StartCoroutine(FadeOut(fromSource, crossFadeDuration));
        StartCoroutine(FadeIn(toSource, 1f, crossFadeDuration));
    }

    private IEnumerator FadeOut(AudioSource source, float duration)
    {
        Debug.Log($"{name} Starting to fade out");
        float startVolume = source.volume;
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            source.volume = Mathf.Lerp(startVolume, 0, t / duration);
            yield return null;
        }
        Debug.Log($"{name} Done fading out");
        source.Stop();
        source.volume = startVolume;
    }

    private IEnumerator FadeIn(AudioSource source, float targetVolume, float duration)
    {
        Debug.Log($"{name} Starting to fade in");
        source.volume = 0;
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            source.volume = Mathf.Lerp(0, targetVolume, t / duration);
            yield return null;
        }
        Debug.Log($"{name} Done fading in");
    }
}