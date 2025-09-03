using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SoundData))]
public class SoundDataEditor : Editor
{
    private SerializedProperty _sounds;
    private Dictionary<int, bool> _expandedStates = new Dictionary<int, bool>();
    private bool _willUpdate = false;

    private void OnEnable()
    {
        _sounds = serializedObject.FindProperty("sounds");
        _willUpdate = false;
    }

    private void OnDisable()
    {
        if (_willUpdate)
        {
            SoundEnumGenerator.Generate();
        }
    }

    void OnDestroy()
    {
        SoundEnumGenerator.Generate();
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.BeginVertical("box");
        {

            for (int i = 0; i < _sounds.arraySize; i++)
            {
                SerializedProperty element = _sounds.GetArrayElementAtIndex(i);
                var nameProp = element.FindPropertyRelative("name");
                var clipProp = element.FindPropertyRelative("clip");

                if (!_expandedStates.ContainsKey(i))
                    _expandedStates[i] = false;

                EditorGUILayout.BeginHorizontal();
                {
                    GUIContent icon = EditorGUIUtility.IconContent("AudioSource Icon");
                    GUILayout.Label(icon, GUILayout.Width(18), GUILayout.Height(18));

                    string soundName = nameProp.stringValue;
                    int buttonWidth = EditorGUIHelper.GetButtonWidthFromString(soundName);

                    if (EditorGUIHelper.ClickButton(soundName, 25, buttonWidth, _expandedStates[i]))
                    {
                        _expandedStates[i] = !_expandedStates[i];
                    }

                    if (clipProp.objectReferenceValue is AudioClip clip)
                    {
                        string buttonLabel;
                        if (AudioPreviewSource.IsPlaying(clip))
                        {
                            buttonLabel = "⏹";
                        }
                        else
                        {
                            buttonLabel = "▶";
                        }
                        if (EditorGUIHelper.ClickButton(buttonLabel, 25, 30))
                        {
                            AudioPreviewSource.PlayOrStopClip(clip, element);
                        }
                    }

                    GUILayout.Space(5);
                }
                EditorGUILayout.EndHorizontal();

                if (_expandedStates[i])
                {
                    OpeningSound(i, element);
                }
            }
        }
        EditorGUILayout.EndVertical();

        if (EditorGUIHelper.ClickButton("Add Sound"))
        {
            AddSound();
        }

        bool hasChanged = serializedObject.ApplyModifiedProperties();

        if (hasChanged)
            _willUpdate = true;
    }

    private void OpeningSound(int index, SerializedProperty element)
    {
        var clipProp = element.FindPropertyRelative("clip");
        var nameProp = element.FindPropertyRelative("name");
        var volumeProp = element.FindPropertyRelative("volume");
        var minPitchProp = element.FindPropertyRelative("minPitch");
        var maxPitchProp = element.FindPropertyRelative("maxPitch");

        float minPitch = minPitchProp.floatValue;
        float maxPitch = maxPitchProp.floatValue;

        EditorGUILayout.BeginVertical("box");
        {
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField("🎵 Audio Clip Settings", EditorStyles.boldLabel);

                // Always set the name to the clip name.
                if (clipProp.objectReferenceValue != null)
                {
                    AudioClip clip = clipProp.objectReferenceValue as AudioClip;
                    nameProp.stringValue = clip.name;
                }

                if (EditorGUIHelper.ClickRemoveButton())
                {
                    _sounds.DeleteArrayElementAtIndex(index);
                    EditorGUILayout.EndHorizontal();
                    return;
                }

            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.PropertyField(element.FindPropertyRelative("clip"));
            var newClip = clipProp.objectReferenceValue as AudioClip;
            if (newClip != null && IsClipAlreadyAssigned(newClip, index))
            {
                Debug.LogWarning($"{newClip.name} is already in use stupid");
                clipProp.objectReferenceValue = null;
            }
            else if (newClip != null)
            {
                nameProp.stringValue = newClip.name;
            }

            EditorGUILayout.PropertyField(element.FindPropertyRelative("mixer"));

            EditorGUILayout.Space(5);
            EditorGUILayout.LabelField("Volume & Pitch", EditorStyles.boldLabel);
            EditorGUILayout.Space(5);

            volumeProp.floatValue = EditorGUILayout.Slider(
                new GUIContent("Volume", "(0 = mute, 1 = full volume)"),
                volumeProp.floatValue, 0.0f, 1.0f
                );

            EditorGUILayout.LabelField("Pitch Range");
            EditorGUILayout.MinMaxSlider(ref minPitch, ref maxPitch, 0.5f, 2.0f);

            EditorGUILayout.BeginHorizontal();
            {
                GUILayout.FlexibleSpace();
                {
                    minPitch = (float)Math.Round(minPitch, 2);
                    maxPitch = (float)Math.Round(maxPitch, 2);

                    EditorGUILayout.LabelField("Min Pitch", GUILayout.Width(65));
                    minPitch = EditorGUILayout.FloatField(minPitch, GUILayout.Width(50));

                    GUILayout.Space(20);

                    EditorGUILayout.LabelField("Max Pitch", GUILayout.Width(65));
                    maxPitch = EditorGUILayout.FloatField(maxPitch, GUILayout.Width(50));

                }
                GUILayout.FlexibleSpace();
            }
            EditorGUILayout.EndHorizontal();

            if (EditorGUIHelper.ClickButton("Reset Pitch", 25, 75))
            {
                minPitch = 1f;
                maxPitch = 1f;
            }

            minPitchProp.floatValue = Mathf.Clamp(minPitch, 0.5f, maxPitch);
            maxPitchProp.floatValue = Mathf.Clamp(maxPitch, minPitch, 2.0f);

        }
        EditorGUILayout.EndVertical();
    }

    private void AddSound()
    {
        _sounds.InsertArrayElementAtIndex(_sounds.arraySize);
        SerializedProperty newElement = _sounds.GetArrayElementAtIndex(_sounds.arraySize - 1);

        newElement.FindPropertyRelative("name").stringValue = "Empty Clip";
        newElement.FindPropertyRelative("clip").objectReferenceValue = null;
        newElement.FindPropertyRelative("mixer").objectReferenceValue = null;
        newElement.FindPropertyRelative("volume").floatValue = 1f;
        newElement.FindPropertyRelative("minPitch").floatValue = 1f;
        newElement.FindPropertyRelative("maxPitch").floatValue = 1f;
    }

    private bool IsClipAlreadyAssigned(AudioClip clip, int currentIndex)
    {
        for (int i = 0; i < _sounds.arraySize; i++)
        {
            if (i == currentIndex) continue;

            SerializedProperty otherElement = _sounds.GetArrayElementAtIndex(i);
            var otherClip = otherElement.FindPropertyRelative("clip").objectReferenceValue as AudioClip;

            if (otherClip == clip)
            {
                return true;
            }
        }
        return false;
    }
}