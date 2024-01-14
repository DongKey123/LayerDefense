using System.Collections.Generic;
using UnityEngine;


public class GameObjectPool<T> where T : MonoBehaviour 
{
    private Stack<T> poolObjects = new Stack<T>();

    private int poolSize;
    private Transform parent;
    private T poolObjectPrefab;

    public void Initialize(T poolObject,Transform poolObjectParent,int size = 10)
    {
        poolObjectPrefab = poolObject;

        if(poolObjectPrefab == null)
        {
            Debug.LogError("Prefab Not Found");
            return;
        }

        parent = poolObjectParent;
        poolSize = size;

        for(int i =0;i<poolSize; i++)
        {
            T obj = CreatePoolObject();
#if UNITY_EDITOR
            obj.transform.SetParent(parent);
#endif
            obj.gameObject.SetActive(false);
            poolObjects.Push(obj);
        }
    }

    public void Push(T poolObject)
    {
        poolObject.gameObject.SetActive(false);
#if UNITY_EDITOR
        poolObject.transform.SetParent(parent);
#endif
        poolObjects.Push(poolObject);
    }

    public T Pop()
    {
        T poolObject;
        if (poolObjects.Count > 0)
            poolObject = poolObjects.Pop();
        else
            poolObject = CreatePoolObject();

        poolObject.gameObject.SetActive(true);

        return poolObject;
    }


    private T CreatePoolObject()
    {
        T createObj = GameObject.Instantiate<T>(poolObjectPrefab);
        return createObj;
    }
}