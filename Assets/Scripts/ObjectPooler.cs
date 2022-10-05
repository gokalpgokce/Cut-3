using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    public List<GameObject> pooledObjects = new List<GameObject>();
    public int amountPool;
    public GameObject prefabToPool;
    
    
    public void WarmUp()
    {
        for (int i = 0; i < amountPool; i++)
        {
            GameObject obj = Instantiate(prefabToPool);
            obj.SetActive(false);
            obj.transform.parent = gameObject.transform;
            pooledObjects.Add(obj);
        }
    }

    public GameObject Get()
    {
        GameObject obj = pooledObjects[pooledObjects.Count-1];
        obj.SetActive(true);
        pooledObjects.RemoveAt(pooledObjects.Count-1);
        return obj;
    }

    public void Put(GameObject go)
    {
        pooledObjects.Add(go);
        go.transform.parent = gameObject.transform;
        go.SetActive(false);
    }
}
