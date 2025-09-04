using UnityEngine;

public class Food : MonoBehaviour
{
    [SerializeField] float foodValue;

    Collider collider;

    private void Awake()
    {
        collider = GetComponent<Collider>();
        if (collider == null)
        {
            Debug.LogWarning("A Food object could not finds its collider");
            Destroy(gameObject);
        }
    }

    public float Eat(float eatingAmount, Proboscis proboscis)
    {
        foodValue -= eatingAmount;
        if (foodValue < 0)
        {
            Debug.Log(foodValue + ", " + eatingAmount + ", " + (eatingAmount + foodValue));
            DestroySelf();
            proboscis.RemoveFood(collider);
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
