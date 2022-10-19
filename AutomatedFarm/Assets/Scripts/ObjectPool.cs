using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
/// This class create pool of itens. It stores them for later use.
/// Instantiate is expensive in large scale.
/// This class reuse objects that otherwise would be destroyed.
///</summary>
public class ObjectPool : Singleton<ObjectPool>
{
    public Queue<GameObject> pool = new Queue<GameObject>();
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

        if(master.TryGetValue(key, out pool))
        {
            pool.Enqueue(go);
        }
    }

    //Get from the pool
    public GameObject GrabFromPool(string key, GameObject go)
    {
        if(master.TryGetValue(key, out pool))
        {
            if(pool.Count > 0)
            {
                GameObject obj = pool.Dequeue();
                // obj.GetComponent<ConveyorItem>().FreshSpawnItem();
                return obj;
            }
            else
            {
                AddToPool(key, go);
                return Instantiate(pool.Dequeue());
            }
        }
        else
        {
            AddToPool(key, go);
            return Instantiate(pool.Dequeue());
        }
    }
}