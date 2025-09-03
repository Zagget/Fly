using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public static class AudioPreviewSource
{
    private static AudioSource previewAudioSource;
    private static AudioClip currentClip;

    public static void PlayOrStopClip(AudioClip clip, SerializedProperty element)
    {
        if (clip == null) return;

        EnsurePreviewAudioSourceExists();

        if (IsPlaying(clip))
        {
            StopClip();
        }
        else
        {

            var minPitchProp = element.FindPropertyRelative("minPitch");
            var maxPitchProp = element.FindPropertyRelative("maxPitch");

            float minPitch = minPitchProp.floatValue;
            float maxPitch = maxPitchProp.floatValue;

            var volumeprop = element.FindPropertyRelative("volume");

            float randomPitch = Random.Range(minPitch, maxPitch);

            previewAudioSource.clip = clip;
            previewAudioSource.loop = false;
            previewAudioSource.volume = volumeprop.floatValue;
            previewAudioSource.pitch = randomPitch;

            previewAudioSource.Play();
            currentClip = clip;
        }
    }

    public static void StopClip()
    {
        if (previewAudioSource != null)
        {
            previewAudioSource.Stop();
            currentClip = null;
        }
    }

    public static bool IsPlaying(AudioClip clip)
    {
        return previewAudioSource != null &&
               previewAudioSource.isPlaying &&
               previewAudioSource.clip == clip;
    }

    private static void EnsurePreviewAudioSourceExists()
    {
        if (previewAudioSource == null)
        {
            GameObject previewObject = GameObject.Find("PreviewSoundSource");
            if (previewObject == null)
            {
                previewObject = new GameObject("PreviewSoundSource");
                previewObject.hideFlags = HideFlags.HideAndDontSave;
                previewAudioSource = previewObject.AddComponent<AudioSource>();
            }
            else
            {
                previewAudioSource = previewObject.GetComponent<AudioSource>() ??
                                     previewObject.AddComponent<AudioSource>();
            }
        }
    }

    static AudioPreviewSource()
    {
        EditorApplication.playModeStateChanged += (state) =>
        {
            if (state == PlayModeStateChange.ExitingEditMode && previewAudioSource != null)
            {
                StopClip();
            }
        };
    }
}
