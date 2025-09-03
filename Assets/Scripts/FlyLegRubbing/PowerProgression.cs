using UnityEngine;
using System.Collections.Generic;
/// <summary>
/// Handles the power selection and progression.
/// </summary>
public class PowerProgression : MonoBehaviour
{
    private static PowerProgression _instance;
    public static PowerProgression Instance {  get { return _instance; } }
    public Powers currentPower { private set; get; }

    private float energyLevel;
    private int powerLevel;

    [SerializeField] private List<Powers> sortedPowers;

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
        if (Mathf.FloorToInt(energyLevel) > powerLevel)
        {
            powerLevel++;
        }
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
            return true;
        }

        return false;
    }
}
/// <summary>
/// All the powers in the game, 0 is None. This is in PowerProgression Script.
/// </summary>
public enum Powers
{
    None,
    Dash,
}