using UnityEngine;


[CreateAssetMenu(fileName = "SettingsData", menuName = "Scriptable Objects/SettingsData")]
public class SettingsData : ScriptableObject
{
    [SerializeField] RotationSetting currentRotation;
}