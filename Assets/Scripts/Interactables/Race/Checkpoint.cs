using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [HideInInspector] public RaceHolder holder;
    new Collider collider;

    private void Awake()
    {
        collider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (!other.TryGetComponent<Rigidbody>(out Rigidbody rigidbody)) return;
        holder.CheckPointReached(transform.GetSiblingIndex(), collider, rigidbody);
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
