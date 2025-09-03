using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a unique identifier for a sound, including its SoundData group and song name.
/// </summary>
public struct SoundIdentifier
{
    public string dataName;
    public string songName;
}

/// <summary>
/// Helper class providing utility methods for working with SoundData and sound identifiers.
/// </summary>
public static class SoundHelper
{
    /// <summary>
    /// Creates a SoundIdentifier from a given enum representing a sound.
    /// </summary>
    /// <typeparam name="T">An enum type representing a sound group.</typeparam>
    /// <param name="song">The enum value representing a specific sound.</param>
    /// <returns>A SoundIdentifier containing the SoundData name and sound name.</returns>
    public static SoundIdentifier GetSoundIdentifiers<T>(T song) where T : struct, System.Enum
    {
        return new SoundIdentifier
        {
            dataName = typeof(T).Name,
            songName = song.ToString()
        };
    }

    /// <summary>
    /// Finds the SoundData with the specified name from a list of SoundData.
    /// </summary>
    /// <param name="allData">The list of all loaded SoundData.</param>
    /// <param name="soundDataName">The name of the SoundData to find.</param>
    /// <returns>The matching SoundData, or null if not found.</returns>
    public static SoundData GetSoundData(List<SoundData> allData, string soundDataName)
    {
        foreach (var soundData in allData)
        {
            if (soundData.name == soundDataName)
                return soundData;
        }
        return null;
    }

    /// <summary>
    /// Finds a specific SoundEntry inside a SoundData by name.
    /// </summary>
    /// <param name="data">The SoundData to search in.</param>
    /// <param name="soundName">The name of the sound to find.</param>
    /// <returns>The matching SoundEntry, or null if not found.</returns>
    public static SoundData.SoundEntry GetSound(SoundData data, string soundName)
    {
        foreach (var sound in data.sounds)
        {
            if (sound.name == soundName)
                return sound;
        }
        return null;
    }

    /// <summary>
    /// Validates a SoundData object to ensure it is not null and contains sounds.
    /// Logs warnings if invalid.
    /// </summary>
    /// <param name="soundData">The SoundData to validate.</param>
    /// <param name="soundDataName">The name of the SoundData (used for logging).</param>
    /// <param name="sourceName">The name of the calling class or context (used for logging).</param>
    /// <returns>True if the SoundData is valid; otherwise, false.</returns>
    public static bool IsSoundDataValid(SoundData soundData, string soundDataName, String sourceName)
    {
        if (soundData == null)
        {
            Debug.LogWarning($"{sourceName} SoundData {soundDataName} is null, have you checked Addressable and added Sound to its label?");
            return false;
        }

        if (soundData.sounds.Length == 0)
        {
            Debug.LogWarning($"{sourceName} No sounds in {soundDataName}");
            return false;
        }

        return true;
    }
}
