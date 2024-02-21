using System;
using System.Collections.Generic;
using UnityEngine;

public interface IObject<T>
{
    Action<T> returnPool { get; set; }
}

public class ObjectPool<T> : MonoBehaviour
    where T : MonoBehaviour, IObject<T>
{
    T prefab;
    Transform parent;
    Stack<T> storage;

    public void Initialized(string prefabName, int firstCount)
    {
        prefab = Resources.Load<T>($"Prefabs/{prefabName}");
        parent = new GameObject("parent").transform;
        parent.SetParent(transform);
        parent.gameObject.SetActive(false);
        storage = new Stack<T>();

        CreateItemObject(firstCount);
    }
    private void CreateItemObject(int count = 1)
    {
        for (int i = 0; i < count; i++)
        {
            T newObject = Instantiate(prefab, parent);
            newObject.returnPool = ReturnPool;
            storage.Push(newObject);
        }
    }

    public T GetObject()
    {
        if (storage.Count <= 0)
            CreateItemObject();

        T obj = storage.Pop();
        obj.transform.SetParent(null);
        return obj;
    }
    private void ReturnPool(T target)
    {
        target.transform.SetParent(parent);
        storage.Push(target);
    }

}
