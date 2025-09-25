using UnityEngine;

[CreateAssetMenu(fileName = "SettingsData", menuName = "Scriptable Objects/SettingsData")]
public class SettingsData : ScriptableObject
{
    [SerializeField] public RotationSetting currentRotation;

    private float playerArmLength;
}