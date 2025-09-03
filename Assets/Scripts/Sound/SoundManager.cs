using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Collections;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;
    public static SoundManager Instance { get { return instance; } }

    [SerializeField] public AudioMixer audioMixer;
    [SerializeField] public AudioMixerVolumeData mixerData;
    [SerializeField, Range(1, 100)] private int _amountOfAudioSources = 40;
    [SerializeField, Range(1, 100)] private int _amountOfAudioSourcesLoop = 5;

    private List<SoundData> _currentSoundData = new List<SoundData>();
    private AudioSourcePool _oneShotPool;
    private AudioSourcePool _loopPool;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
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

    private void OnSoundsLoaded(AsyncOperationHandle<IList<SoundData>> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            Debug.Log($"{name} All SoundData loaded successfully!");
            _currentSoundData = new List<SoundData>(handle.Result);
            mixerData.ApplyAllVolume(audioMixer);
        }
        else
        {
            Debug.LogError($"{name} Failed to load SoundData assets.");
        }
    }

    private void CreateAudioSources()
    {
        GameObject audioSourceContainerOneShot = new GameObject("AudioSourcesOneShot");
        GameObject audioSourceContainerLoops = new GameObject("AudioSourcesLoop");

        audioSourceContainerOneShot.transform.SetParent(transform);
        audioSourceContainerLoops.transform.SetParent(transform);

        _oneShotPool = new AudioSourcePool(audioSourceContainerOneShot, _amountOfAudioSources);
        _loopPool = new AudioSourcePool(audioSourceContainerLoops, _amountOfAudioSourcesLoop);
    }

    /// <summary>
    /// Plays a sound identified by the given enum value.
    /// </summary>
    /// <typeparam name="T">An enum type representing the sound identifier.</typeparam>
    /// <param name="sound">The enum value specifying which sound to play.</param>
    /// <param name="loop">If true, the sound will loop continuously; otherwise, it will play once.</param>
    public void PlaySound<T>(T sound, bool loop = false) where T : struct, System.Enum
    {
        var sid = SoundHelper.GetSoundIdentifiers(sound);

        SoundData soundData = SoundHelper.GetSoundData(_currentSoundData, sid.dataName);

        if (!SoundHelper.IsSoundDataValid(soundData, sid.dataName, this.name)) return;

        if (sid.songName == "Random")
        {
            SoundData.SoundEntry randomEntry = soundData.sounds[Random.Range(0, soundData.sounds.Length)];
            PlayClip(randomEntry, loop);
            return;
        }

        SoundData.SoundEntry entry = SoundHelper.GetSound(soundData, sid.songName);

        if (entry == null)
        {
            Debug.Log($"{name} {sid.songName} not found in {sid.dataName}");
            return;
        }

        PlayClip(entry, loop);
    }

    private void PlayClip(SoundData.SoundEntry entry, bool loop)
    {
        Debug.Log($"{name} in playclip");
        AudioSource currentSource;

        if (loop)
        {
            currentSource = _loopPool.GetAvailable();
            _loopPool.AddInLookUp(currentSource, entry.name);
            Debug.Log($"{name} {entry.name} added to loop");
        }
        else
        {
            currentSource = _oneShotPool.GetAvailable();
        }

        currentSource.loop = loop;
        currentSource.clip = entry.clip;
        currentSource.volume = entry.volume;

        float randomPitch = Random.Range(entry.minPitch, entry.maxPitch);
        currentSource.pitch = randomPitch;

        currentSource.outputAudioMixerGroup = entry.mixer;
        currentSource.Play();

        Debug.Log($"{name} Played {entry.name} on {entry.mixer} looping: {loop}");
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

        AudioSource sound = _loopPool.GetSourceBySoundName(sid.songName);
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
        AudioSource fromSource = _loopPool.GetSourceBySoundName(fromEntry.name);
        if (fromSource == null)
        {
            Debug.Log($"{name} No active source for {fromEntry.name}, playing {toEntry.name} immediately.");
            PlayClip(toEntry, loop: true);
            return;
        }

        // Create new source for 'to' clip
        AudioSource toSource = _loopPool.GetAvailable();
        toSource.clip = toEntry.clip;
        toSource.loop = true;
        toSource.outputAudioMixerGroup = toEntry.mixer;
        toSource.volume = 0f;
        _loopPool.AddInLookUp(toSource, toEntry.name);
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