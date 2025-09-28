using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RotationSetting
{
    [SerializeField] private float degrees;
    [SerializeField] private float smoothness = 0.25f;

    private int degreeIndex;
    private int smoothIndex;
    public readonly List<float> DegreeValues = new List<float> { 1, 20, 30, 45, 90, 179 };
    public readonly List<float> SmoothnessValues = new List<float> { 0f, 0.15f, 0.2f, 0.4f, 0.5f };


    public event Action<float, float> OnRotationChanged;

    public void Init()
    {
        UpdateIndex();

        OnRotationChanged?.Invoke(degrees, smoothness);
    }

    private void UpdateIndex()
    {
        degreeIndex = DegreeValues.IndexOf(degrees);
        if (degreeIndex < 0) degreeIndex = 0;
        degrees = DegreeValues[degreeIndex];

        smoothIndex = SmoothnessValues.IndexOf(smoothness);
        if (smoothIndex < 0) smoothIndex = 0;
        smoothness = SmoothnessValues[smoothIndex];
    }

    public void SetCustomValues(float newDegrees, float newSmoothness)
    {
        degrees = Mathf.Clamp(newDegrees, 0, 179);
        smoothness = Mathf.Clamp(newSmoothness, 0f, 0.5f);

        OnRotationChanged?.Invoke(degrees, smoothness);
    }

    public void SetPreset(RotationPreset preset)
    {
        switch (preset)
        {
            case RotationPreset.Default:
                SetCustomValues(30, 0.15f);
                break;
            case RotationPreset.Snappy:
                SetCustomValues(30, 0f);
                break;
            case RotationPreset.Smooth:
                SetCustomValues(45, 0.4f);
                break;
        }
        UpdateIndex();
    }

    public void IncrementDegrees(bool increase)
    {
        Debug.Log($"Index {degreeIndex} increase : {increase}");
        if (increase)
            degreeIndex += 1;
        else
            degreeIndex -= 1;

        if (degreeIndex < 0)
            degreeIndex = DegreeValues.Count - 1;
        else if (degreeIndex >= DegreeValues.Count)
            degreeIndex = 0;

        Debug.Log($"Index {degreeIndex} after ifs");


        degrees = DegreeValues[degreeIndex];

        OnRotationChanged?.Invoke(degrees, smoothness);
    }

    public void IncrementSmoothness(bool increase)
    {
        if (increase)
            smoothIndex++;
        else
            smoothIndex--;

        if (smoothIndex < 0)
            smoothIndex = SmoothnessValues.Count - 1;
        else if (smoothIndex >= SmoothnessValues.Count)
            smoothIndex = 0;

        smoothness = SmoothnessValues[smoothIndex];

        OnRotationChanged?.Invoke(degrees, smoothness);
    }
}

public enum RotationPreset
{
    Default,
    Snappy,
    Smooth
}