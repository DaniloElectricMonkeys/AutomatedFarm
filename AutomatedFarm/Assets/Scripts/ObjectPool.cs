using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : Singleton<ObjectPool>
{
    // This class create pool of itens. It stores them for later use.
    // Instantiate is expensive in large scale.
    // This class reuse objects that otherwise would be destroyed.

    public Queue<GameObject> pooling = new Queue<GameObject>();
    public Dictionary<string,Queue<GameObject>> master = new Dictionary<string,Queue<GameObject>>();

    //Creat a new pool ot itens and store it on the dictionary
    public void NewPool(string key)
    {
        Queue<GameObject> newQueue = new Queue<GameObject>();
        master.Add(key, newQueue);
    }


    //Add objects to the pool
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

    //Get from the pool
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