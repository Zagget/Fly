#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

public class SoundEnumGenerator : EditorWindow
{
    [MenuItem("Tools/Generate Sound Enums")]
    public static void Generate()
    {
        string path = "Assets/Scripts/Sound/SoundEnums.cs";

        var guids = AssetDatabase.FindAssets("t:SoundData");

        var soundDataAssets = guids
            .Select(guid => AssetDatabase.LoadAssetAtPath<SoundData>(AssetDatabase.GUIDToAssetPath(guid)))
            .Where(sd => sd != null)
            .OrderBy(sd => sd.name)
            .ToArray();

        using (StreamWriter writer = new StreamWriter(path))
        {
            writer.WriteLine("// This file is auto-generated. Do not edit manually.");
            writer.WriteLine();

            foreach (var soundData in soundDataAssets)
            {
                var sounds = soundData.sounds
                    .Where(s => s.name != "Empty Clip")
                    .OrderBy(s => s.name)
                    .ToArray();

                if (sounds.Length == 0)
                    continue;

                string safeSoundDataName = MakeSafeForCode(soundData.name);
                writer.WriteLine($"public enum {safeSoundDataName}");
                writer.WriteLine("{");


                writer.WriteLine($"     Random,");

                for (int i = 0; i < sounds.Length; i++)
                {
                    string safeSoundName = MakeSafeForCode(sounds[i].name);
                    string comma = "";
                    if (i < sounds.Length - 1)
                        comma = ",";

                    writer.WriteLine($"     {safeSoundName}{comma}");
                }

                writer.WriteLine("}");
            }
        }
        AssetDatabase.Refresh();
        Debug.Log($"SoundEnums.cs generated with {soundDataAssets.Length} enums.");
    }

    private static string MakeSafeForCode(string input)
    {
        // Replace invalid chars and ensure it starts with a letter or underscore
        string safe = Regex.Replace(input, @"[^a-zA-Z0-9_]", "_");

        if (!char.IsLetter(safe, 0) && safe.Length > 0)
            safe = "_" + safe;

        return safe;
    }
}
#endif