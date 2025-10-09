using System.Data.Common;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RaceUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textBox;
    [SerializeField] Image targetImage;
    [SerializeField] Color start;
    [SerializeField] Color end;
    [SerializeField] AnimationCurve curve;

    [Header("Anim")]
    Animator animator;
    [SerializeField] AnimationClip countdown;


    private void Awake()
    {
        targetImage.color = start;
        animator = GetComponent<Animator>();
        animator.enabled = false;
    }

    public void ChargeUp(float value)
    {
        if (targetImage != null) targetImage.color = Color.Lerp(start, end, curve.Evaluate(value));
    }

    public void StartCountDown(float time)
    {
        animator.enabled = true;
        animator.speed = countdown.length / time;
        animator.Play(countdown.name);
    }

    public void SetText(string text)
    {
        textBox.text = text;
    }

    public void DisableAnimator()
    {
        animator.enabled = false;
    }
}
