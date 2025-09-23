using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "MixerData", menuName = "Scriptable Objects/Sound/AudioMixerVolumeData")]
public class AudioMixerVolumeData : ScriptableObject
{
    [System.Serializable]
    public struct VolumeParameter
    {
        public string exposedParameterName;

        [Range(0.00001f, 1f)]
        public float volumeLevel;
    }

    public List<VolumeParameter> volumeParameters = new List<VolumeParameter>();

    /// <summary>
    /// Applies volume to all exposed parameters.
    /// </summary>
    public void ApplyAllVolume(AudioMixer mixer)
    {
        foreach (var setting in volumeParameters)
        {
            float dbVolume = Mathf.Log10(setting.volumeLevel) * 20f;
            mixer.SetFloat(setting.exposedParameterName, dbVolume);
        }
    }

    /// <summary>
    /// Applies the current volume to a single exposed parameter.
    /// </summary>
    public void ApplyVolume(AudioMixer mixer, VolumeParameter vc)
    {
        float dbVolume = Mathf.Log10(vc.volumeLevel) * 20f;
        mixer.SetFloat(vc.exposedParameterName, dbVolume);
    }
}