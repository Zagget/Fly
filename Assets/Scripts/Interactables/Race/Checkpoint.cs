using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [HideInInspector] public RaceHolder holder;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (!other.TryGetComponent<Rigidbody>(out Rigidbody rigidbody)) return;
        holder.CheckPointReached(transform.GetSiblingIndex(), transform, rigidbody);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
