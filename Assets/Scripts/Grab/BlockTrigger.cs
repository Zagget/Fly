using UnityEngine;

public class BlockTrigger : MonoBehaviour
{

    Grabable grabable;

    private void Awake()
    {
        grabable = GetComponentInParent<Grabable>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Player" && other.isTrigger != true)
        {
            grabable.blockers.Add(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag != "Player" && other.isTrigger != true)
        {
            grabable.blockers.Remove(other);
        }
    }


}
