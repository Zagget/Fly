using UnityEngine;
using UnityEngine.Video;

public enum TutorialID
{
    Welcome,
    Rotation,
    Flying,
    Hover,
    Walking,
    Eating

}

[CreateAssetMenu(fileName = "NewTutorial", menuName = "Scriptable Objects/TutorialData")]
public class TutorialData : ScriptableObject
{
    public TutorialID id;
    public VideoClip clip;
    public string header;
    public string description;
}