using System.Collections;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    private static AnimationManager _instance;
    public static AnimationManager Instance { get { return _instance; } }

    void Awake()
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
    public IEnumerator WaitForAnimation(Animator animator, string stateName)
    {
        yield return new WaitUntil(() =>
            animator.GetCurrentAnimatorStateInfo(0).IsName(stateName));
        yield return new WaitUntil(() =>
            !animator.GetCurrentAnimatorStateInfo(0).IsName(stateName)); 
    }
}
