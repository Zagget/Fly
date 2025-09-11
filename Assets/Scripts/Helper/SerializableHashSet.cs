using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SerializableHashSet<T> : HashSet<T>, ISerializationCallbackReceiver
{
    [SerializeField] List<T> list = new();

    public void OnBeforeSerialize()
    {
        list.Clear();
        foreach (T item in this)
        {
            list.Add(item);
        }
    }

    public void OnAfterDeserialize()
    {
        this.Clear();

        foreach (T item in list)
        {
            this.Add(item);
        }
    }
}
