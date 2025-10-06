using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;





#if UNITY_EDITOR
using UnityEditor;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(ShatteredObject))]
public class ShatteredObjectEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        ShatteredObject shatteredObject = (ShatteredObject)target;

        if (GUILayout.Button("Give rigidbodies and colliders"))
        {
            Undo.RecordObject(shatteredObject, "Adding rigidbodies and colliders");

            Transform child;
            for (int i = 0; i < shatteredObject.transform.childCount; i++)
            {
                child = shatteredObject.transform.GetChild(i);
                if (!child.TryGetComponent<Rigidbody>(out Rigidbody rigidbody))
                {
                    rigidbody = child.AddComponent<Rigidbody>();
                }
                if (!shatteredObject.rigidbodies.Contains(rigidbody)) shatteredObject.rigidbodies.Add(rigidbody);

                if (rigidbody.transform.GetComponent<BoxCollider>() == null) child.AddComponent<BoxCollider>();
            }

            PrefabUtility.RecordPrefabInstancePropertyModifications(shatteredObject);
            EditorSceneManager.SaveScene(SceneManager.GetActiveScene());
        }

        if (GUILayout.Button("Remove rigidbodies and colliders"))
        {
            Transform child;
            shatteredObject.rigidbodies = new();

            for (int i = 0; i < shatteredObject.transform.childCount; i++)
            {
                child = shatteredObject.transform.GetChild(i);
                if (child.TryGetComponent<Rigidbody>(out Rigidbody rigidbody)) DestroyImmediate(rigidbody);
                if (child.TryGetComponent<BoxCollider>(out BoxCollider boxCollider)) DestroyImmediate(boxCollider);
            }
        }
    }
}

#endif


public class ShatteredObject : MonoBehaviour
{
    [SerializeField] float forceMultiplier;
    [SerializeField] float forceBase;
    [SerializeField] float falseLift;

    public List<Rigidbody> rigidbodies = new();

    [Header("Decay")]
    [SerializeField] AnimationCurve decayCurve;
    [SerializeField] float decayTime;

    float decayStart;

    public void Explode(Vector3 point, Vector3 force)
    {
        decayStart = Time.time;
        float magnitude = force.sqrMagnitude * forceMultiplier + forceBase;
        foreach (Rigidbody rigidbody in rigidbodies)
        {
            rigidbody.AddExplosionForce(magnitude, point, Mathf.Infinity, falseLift, ForceMode.Acceleration);
            StartCoroutine(decay(rigidbody.transform));
        }
        Destroy(gameObject, decayTime);
    }

    IEnumerator decay(Transform decayObject)
    {
        float value;
        Vector3 scale = decayObject.localScale;
        while (true)
        {
            decayObject.position = Vector3.zero;
            value = (Time.time - decayStart) / decayTime;
            decayObject.localScale = scale * decayCurve.Evaluate(value);
            if (value > 1) { break; }
            yield return new WaitForEndOfFrame();
        }
        Destroy(decayObject.gameObject);
        //if you get error, check rigidbody list to make sure it is referencing the correct game objects.
    }
}
