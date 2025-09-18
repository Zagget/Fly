using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PowerManager : MonoBehaviour
{
    public static PowerManager Instance { get; private set; }

    [SerializeField] powerPair[] powerMapping;
    RigManager rigManager;
    powerPair currentPower;
    public Action continues;
    public static Action<powerPair> onChangePower;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
    }

    private void Start()
    {
        rigManager = RigManager.instance;
        PowerProgression.Instance.onPowerChange += FindPower;
    }

    public void ActivatePower(InputAction.CallbackContext context, PlayerController playerController)
    {
        if (currentPower == null || currentPower.basePower == null) return;
        float charge = LegRubbing.Instance.TotalRubbing;
        currentPower.basePower.Activate(rigManager, charge, this, playerController);
    }

    void FindPower(Powers power)
    {
        for (int i = 0; i < powerMapping.Length; i++)
        {
            if (power == powerMapping[i].power)
            {
                if (currentPower != null && currentPower.basePower != null) currentPower.basePower.DeactivateToggle();
                currentPower = powerMapping[i];
                onChangePower?.Invoke(currentPower);
                return;
            }
        }
    }

    private void Update()
    {
        continues?.Invoke();
    }

    private void OnDisable()
    {
        PowerProgression.Instance.onPowerChange -= FindPower;
    }
}

[Serializable]
public class powerPair
{
    public Powers power;
    public BasePower basePower;
}
