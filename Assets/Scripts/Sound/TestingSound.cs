using UnityEngine;
using UnityEngine.InputSystem;
public class TestingSound : MonoBehaviour
{
    public AudioSource testSource;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            SoundManager.instance.PlaySound(Test123.CoinDropping, testSource);
        }
    }
}
