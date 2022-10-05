using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyEnums;
using System;

public class BoilerMachine : OutputMachine
{
    Dictionary<string, List<GameObject>> resourcesInTheMachine = new Dictionary<string, List<GameObject>>();
    List<GameObject> objList = new List<GameObject>();

    public override void OnResourceEnter(ResourceType type, GameObject obj)
    {
        string key = type.ToString();

        //Add object to the list of its type
        if(resourcesInTheMachine.ContainsKey(key))
        {
            if(resourcesInTheMachine.TryGetValue(key, out objList))
                objList.Add(obj);
        }
        //Add object to the list of its type by creating a new list if it is a new resource type
        else
        {
            List<GameObject> newList = new List<GameObject>();
            newList.Add(obj);
            resourcesInTheMachine.Add(key, newList);
        }

        ObjectPool.Instance.AddToPool(key, obj.gameObject);
        obj.SetActive(false);
        resourceAmount++;
    }

    public override void OutputResource()
    {
        if(resourceAmount <= 0) return;

        if(!isConnected) CheckOutput();
        if(!isConnected) return;

        if(outputType == ResourceType.none)
        {
            Debug.Log("NO RESOURCE SELECTED");
            return;
        }

        if(outputType == ResourceType.variable)
        {
            if(resourcesInTheMachine.Count > 0)
            {
                foreach(var item in resourcesInTheMachine)
                {
                    ResourceType type = (ResourceType)Enum.Parse(typeof(ResourceType), item.Key);

                    switch (type)
                    {
                        case ResourceType.corn:
                            go = ObjectPool.Instance.GrabFromPool("Soil", Library.Instance.boiledCorn);
                        break;
                    }
                    break;
                }
            }
        }

        go.transform.position = outputPoint.transform.position;
        go.transform.rotation = Quaternion.identity;
        go.SetActive(true);

        resourceAmount--;
    }
}
