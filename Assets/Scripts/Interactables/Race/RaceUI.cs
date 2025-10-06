using UnityEngine;
using UnityEngine.UI;

public class RaceUI : MonoBehaviour
{
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
    }

    public void ChargeUp(float value)
    {
        if (targetImage != null) targetImage.color = Color.Lerp(start, end, curve.Evaluate(value));
    }

    public void StartCountDown(float time)
    {
        targetImage.color = Vector4.zero;
        float playbackSpeed = countdown.length / time;
        animator.Play(countdown.name, 0, playbackSpeed);
    }
}
