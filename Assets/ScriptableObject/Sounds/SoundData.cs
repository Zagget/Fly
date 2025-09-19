using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "SoundData", menuName = "Scriptable Objects/Sound/SoundData")]
public class SoundData : ScriptableObject
{
    [System.Serializable]
    public class SoundEntry
    {
        public string name;
        public AudioClip clip;
        public float volume = 1;
        public float minPitch;
        public float maxPitch;
        public AudioMixerGroup mixer;
    }

    public SoundEntry[] sounds;
}