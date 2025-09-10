using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PowerUI : MonoBehaviour
{
    [SerializeField] Slider powerSlider;
    [SerializeField] TextMeshProUGUI powerText;

    private void Start()
    {
        PowerManager.onChangePower += OnChangePower;
        LegRubbing.Instance.chargeChange += OnChangeCharge;
    }

    public void OnChangePower(powerPair powerPair)
    {
        if (powerPair == null) return;
        powerText.text = powerPair.power.ToString();
        if (powerPair.basePower == null) powerSlider.gameObject.SetActive(false);
        else
        {
            powerSlider.gameObject.SetActive(true);
            powerText.transform.parent.gameObject.SetActive(true);
            powerSlider.maxValue = powerPair.basePower.MaximumCharge;
        }

    }

    public void OnChangeCharge(float charge)
    {
        powerSlider.value = charge;
    }

    private void OnDisable()
    {
        PowerManager.onChangePower -= OnChangePower;
        LegRubbing.Instance.chargeChange -= OnChangeCharge;
    }
}
