using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages a pool of reusable AudioSource components to optimize audio playback performance.
/// </summary>
public class AudioSourcePool
{
    private readonly List<AudioSource> _sources = new();
    private Dictionary<AudioSource, string> _lookUp = new();

    /// <summary>
    /// Creates a pool of AudioSources attached to the given container GameObject.
    /// </summary>
    /// <param name="container">The GameObject to which AudioSources will be attached.</param>
    /// <param name="amount">The number of AudioSources to create in the pool.</param>
    public AudioSourcePool(GameObject container, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            AudioSource source = container.AddComponent<AudioSource>();
            source.playOnAwake = false;
            _sources.Add(source);
        }
    }

    /// <summary>
    /// Returns the first available (not currently playing) AudioSource from the pool.
    /// If none are available, returns the first AudioSource with a warning.
    /// </summary>
    /// <returns>An available AudioSource.</returns>
    public AudioSource GetAvailable()
    {
        foreach (var source in _sources)
        {
            if (!source.isPlaying)
                return source;
        }

        Debug.LogWarning($"No available sources returning first one");
        return _sources[0];
    }

    /// <summary>
    /// Adds or updates the lookup entry associating an AudioSource with a sound name.
    /// </summary>
    /// <param name="source">The AudioSource to associate.</param>
    /// <param name="soundName">The name of the sound being played.</param>
    public void AddInLookUp(AudioSource source, string soundName)
    {
        if (_lookUp.ContainsKey(source))
            _lookUp[source] = soundName;
        else
            _lookUp.Add(source, soundName);
    }

    /// <summary>
    /// Finds a currently playing AudioSource by its associated sound name.
    /// </summary>
    /// <param name="soundName">The name of the sound to find.</param>
    /// <returns>The playing AudioSource with the matching sound name, or null if none found.</returns>
    public AudioSource GetSourceBySoundName(string soundName)
    {
        foreach (var pair in _lookUp)
        {
            if (pair.Value == soundName && pair.Key.isPlaying)
            {
                return pair.Key;
            }
        }

        return null;
    }
}