using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : Singleton<ObjectPool>
{
    public Queue<GameObject> pooling = new Queue<GameObject>();
    public Dictionary<string,Queue<GameObject>> master = new Dictionary<string,Queue<GameObject>>();

    public void NewPool(string key)
    {
        Queue<GameObject> newQueue = new Queue<GameObject>();
        master.Add(key, newQueue);
    }

    public void AddToPool(string key, GameObject go)
    {
        if(master.ContainsKey(key) == false)
        {
            NewPool(key);
        }

        if(master.TryGetValue(key, out pooling))
        {
            pooling.Enqueue(go);
        }
    }

    public GameObject GrabFromPool(string key, GameObject go)
    {
        if(master.TryGetValue(key, out pooling))
        {
            if(pooling.Count > 0)
            {
                return pooling.Dequeue();
            }
            else
            {
                AddToPool(key, go);
                return Instantiate(pooling.Dequeue());
            }
        }
        else
        {
            AddToPool(key, go);
            return Instantiate(pooling.Dequeue());
        }
    }
}