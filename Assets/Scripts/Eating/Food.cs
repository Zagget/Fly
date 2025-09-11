using UnityEngine;

public class Food : MonoBehaviour
{
    [SerializeField] float foodValue;

    Collider foodCollider;

    private void Awake()
    {
        foodCollider = GetComponent<Collider>();
        if (foodCollider == null)
        {
            Debug.LogWarning("A Food object could not finds its collider");
            Destroy(gameObject);
        }
    }

    public float Eat(float eatingAmount, Proboscis proboscis = null)
    {
        foodValue -= eatingAmount;
        if (foodValue < 0)
        {
            DestroySelf();
            if (proboscis != null) proboscis.RemoveFood(foodCollider);
            eatingAmount += foodValue;
        }

        return eatingAmount;
    }

    void DestroySelf()
    {
        //Animation or other funny things
        Destroy(gameObject);
    }
}
