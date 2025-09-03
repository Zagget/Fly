using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public enum SoundEditorShowTypes
{
    None,
    GroupVolumes,
}

[CustomEditor(typeof(SoundManager))]
public class SoundManagerEditor : Editor
{
    private SoundEditorShowTypes _currentSelected = SoundEditorShowTypes.None;
    private string _userInput = "Exposed parameter";
    private Dictionary<int, bool> _expandedStates = new Dictionary<int, bool>();

    public override void OnInspectorGUI()
    {
        SoundManager soundManager = (SoundManager)target;

        DrawDefaultInspector();

        EditorGUILayout.Space();

        EditorGUIHelper.DrawToggleButton("Group Volumes", SoundEditorShowTypes.GroupVolumes, ref _currentSelected);

        if (_currentSelected == SoundEditorShowTypes.GroupVolumes)
        {
            var mixerData = soundManager.mixerData;

            if (mixerData == null || mixerData.volumeParameters == null)
            {
                Debug.LogWarning($"{soundManager.name} mixer data is not assigned in SoundManager.");
                return;
            }

            var settings = mixerData.volumeParameters;

            for (int i = 0; i < settings.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();

                if (!_expandedStates.ContainsKey(i))
                    _expandedStates[i] = false;


                var volume = settings[i];
                string name = volume.exposedParameterName;
                int buttonWidth = EditorGUIHelper.GetButtonWidthFromString(name);

                EditorGUILayout.BeginVertical();
                if (EditorGUIHelper.ClickButton(name, 25, buttonWidth, _expandedStates[i]))
                {
                    _expandedStates[i] = !_expandedStates[i];
                }

                EditorGUI.BeginChangeCheck();

                if (_expandedStates[i])
                {
                    EditorGUILayout.BeginHorizontal();
                    volume.exposedParameterName = EditorGUILayout.TextField(volume.exposedParameterName, GUILayout.Width(buttonWidth));

                    if (EditorGUIHelper.ClickRemoveButton())
                        Remove(soundManager, settings[i]);

                    EditorGUILayout.EndHorizontal();
                }

                EditorGUILayout.EndVertical();

                volume.volumeLevel = EditorGUILayout.Slider(volume.volumeLevel, 0.00001f, 1f);

                if (EditorGUI.EndChangeCheck())
                {
                    settings[i] = volume;

                    mixerData.ApplyVolume(soundManager.audioMixer, volume);
                    EditorUtility.SetDirty(mixerData);
                }


                EditorGUILayout.EndHorizontal();
            }

            if (EditorGUIHelper.ClickButton("Add new"))
            {
                AddMixer(soundManager, _userInput);
            }
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(soundManager);
        }
    }



    private void AddMixer(SoundManager soundManager, string name)
    {
        soundManager.mixerData.volumeParameters.Add(new AudioMixerVolumeData.VolumeParameter { exposedParameterName = name, volumeLevel = 1 });
    }

    private void Remove(SoundManager soundManager, AudioMixerVolumeData.VolumeParameter vc)
    {
        soundManager.mixerData.volumeParameters.Remove(vc);
    }

}