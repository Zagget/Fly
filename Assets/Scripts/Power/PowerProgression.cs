using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.InputSystem;

#region editor
#if UNITY_EDITOR
using UnityEditor;
[CustomEditor(typeof(PowerProgression))]
public class PowerProgressionEditor : Editor
{
    public override void OnInspectorGUI()
    {
        PowerProgression powerProgression = (PowerProgression)target;
        base.OnInspectorGUI();
        if (GUILayout.Button("Set debug as real power"))
        {
            powerProgression.SetDebugPower();
        }

        if (GUILayout.Button("Next power"))
        {
            powerProgression.NextPower(new());
        }
    }
}
#endif
#endregion

/// <summary>
/// Handles the power selection and progression.
/// </summary>
public class PowerProgression : MonoBehaviour
{
    private static PowerProgression _instance;
    public static PowerProgression Instance { get { return _instance; } }
    public Powers currentPower { private set; get; }
    public Action<Powers> onPowerChange;

    private float energyLevel;
    private int powerLevel;

    [SerializeField] private List<Powers> sortedPowers;
    public Powers debugPower;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // InputManager.Instance.r_ButtonAAction.started += NextPower;
    }

    private void OnDisable()
    {
        // InputManager.Instance.r_ButtonAAction.started -= NextPower;
    }

    /// <summary>
    /// Add energy so the player can level up and unlock new powers, each int is a new power.
    /// </summary>
    /// <param name="amount"></param>
    public void AddEnergy(float amount)
    {
        energyLevel += amount;
        CheckUnlockReqs();
    }

    /// <summary>
    /// Checks if the player can unlock a new power.
    /// </summary>
    private void CheckUnlockReqs()
    {
        int newPowerLevel = Mathf.FloorToInt(energyLevel);
        newPowerLevel = Mathf.Min(newPowerLevel, sortedPowers.Count - 1);
        if (newPowerLevel > powerLevel)
        {
            powerLevel = newPowerLevel;
            ChangePower(sortedPowers[powerLevel]);
        }
    }

    /// <summary>
    /// Checks for unlocks after setting new energy level
    /// </summary>
    /// <param name="energy"></param>
    public void SetEnergyLevel(float energy)
    {
        energyLevel = energy;
        CheckUnlockReqs();

    }

    /// <summary>
    /// Returns true if the power was successfully changed, false otherwise.
    /// </summary>
    /// <param name="power"></param>
    /// <returns></returns>
    public bool ChangePower(Powers power)
    {
        if (powerLevel >= sortedPowers.IndexOf(power))
        {
            currentPower = power;
            onPowerChange?.Invoke(currentPower);
            return true;
        }

        return false;
    }

    public void NextPower(InputAction.CallbackContext context)
    {
        int indexOfPower = sortedPowers.IndexOf(currentPower);
        currentPower = sortedPowers[(indexOfPower + 1) % (powerLevel + 1)];
        onPowerChange?.Invoke(currentPower);
    }

    /// <summary>
    /// uses the debug power as the new power
    /// </summary>
    /// <param name="powers"></param>
    public void SetDebugPower() 
    {
        currentPower = debugPower;
        onPowerChange?.Invoke(currentPower);
    }
}
/// <summary>
/// All the powers in the game, 0 is None. This is in PowerProgression Script.
/// </summary>


[Serializable]
public enum Powers
{
    None,
    Dash,
    EnergyBlast,
    SpewMaggots
}