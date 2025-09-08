using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;




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

    public void Explode(Vector3 point, Vector3 force)
    {
        Debug.Log(rigidbodies.Count);
        float magnitude = force.sqrMagnitude * forceMultiplier + forceBase;
        foreach (Rigidbody rigidbody in rigidbodies)
        {
            rigidbody.AddExplosionForce(magnitude, point, Mathf.Infinity, falseLift, ForceMode.Acceleration);
        }
    }
}
