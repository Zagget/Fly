using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RaceHUD : MonoBehaviour
{
    public static RaceHUD Instance { get; private set; }

    [SerializeField] GameObject arrow;
    [SerializeField] GameObject chargeHint;
    [SerializeField] Slider chargeSlider;
    [SerializeField] GameObject timerImage;
    [SerializeField] TextMeshProUGUI timer;

    RaceHolder raceHolder;

    RaceHolder.states state = RaceHolder.states.idle;
    float time;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);

        DisableAllElements();
    }

    private void Update()
    {
        switch(state)
        {
            case RaceHolder.states.standby:
                RotateArrow();
                break;
            case RaceHolder.states.racing:
                CountDownClock();
                break;
        }
    }
    
    private void RotateArrow() { }

    private void CountDownClock()
    {
        time -= Time.deltaTime;
        if (time < 0)
        {
            raceHolder.ExitRace();
            ExitRace();
        }
        else timer.text = time.ToString("0");
    }

    private void DisableAllElements()
    {
        arrow.SetActive(false);
        chargeHint.SetActive(false);
        chargeSlider.gameObject.SetActive(false);
        timerImage.SetActive(false);
    }

    public void StartStandby()
    {
        state = RaceHolder.states.standby;
        arrow.SetActive(true);
    }

    public void StartCountdown(float time, RaceHolder raceHolder)
    {
        this.raceHolder = raceHolder;
        state = RaceHolder.states.countdown;
        this.time = time;
        SetSliderValue(0);
        LegRubbing.Instance.chargeChange += SetSliderValue;
        arrow.SetActive(false);
        chargeSlider.gameObject.SetActive(true);
        chargeHint.SetActive(true);
        timerImage.SetActive(true);
    }

    public void StartRace()
    {
        state = RaceHolder.states.racing;
        chargeHint.SetActive(false);
    }

    public void ExitRace()
    {
        state = RaceHolder.states.idle;
        LegRubbing.Instance.chargeChange -= SetSliderValue;
        DisableAllElements();
    }

    public void SetSliderValue(float value) { chargeSlider.value = value; }

}
