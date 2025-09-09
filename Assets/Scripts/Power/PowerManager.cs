using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PowerManager : MonoBehaviour
{
    [SerializeField] powerPair[] powerMapping;
    Rigidbody currentRigidbody;
    public Action continues;

    private void Start()
    {
        currentRigidbody = RigManager.instance.currentRb;
        //InputManager.Instance.activatePower.performed += ActivatePower;
    }

    public void ActivatePower(InputAction.CallbackContext context)
    {
        Debug.Log("KILL ME WITH FIRE");
        /*
        Powers power = PowerProgression.Instance.currentPower;

        if (FindPower(power, out int i))
        {
            if (powerMapping[i].basePower == null) return;
            float charge = LegRubbing.Instance.ResetRubbing();
            powerMapping[i].basePower.Activate(currentRigidbody, charge, this);
        }
        else Debug.LogWarning("Power does not exist inside of power mapping: " + power);
        */
    }

    bool FindPower(Powers power, out int index)
    {
        index = -1;
        for (int i = 0; i < powerMapping.Length; i++)
        {
            index = i;
            if (power == powerMapping[i].power) return true;
        }
        return false;
    }

    private void Update()
    {
        continues?.Invoke();
    }

    private void OnDisable()
    {
        //InputManager.Instance.activatePower.performed -= ActivatePower;
    }
}

[Serializable]
struct powerPair
{
    public Powers power;
    public BasePower basePower;
}
