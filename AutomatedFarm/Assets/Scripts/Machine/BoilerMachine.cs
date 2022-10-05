using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyEnums;
using System;

public class BoilerMachine : OutputMachine
{
    Dictionary<string, int> resourcesInTheMachine = new Dictionary<string, int>();
    int quantity;

    public override void OnResourceEnter(ResourceType type, GameObject obj)
    {
        string key = type.ToString();

        //Add object to the list of its type
        if(resourcesInTheMachine.ContainsKey(key))
        {
            resourcesInTheMachine[key] += 1;
        }
        //Add object to the list of its type by creating a new list if it is a new resource type
        else
        {
            int q = 1;
            resourcesInTheMachine.Add(key, q);
        }

        ObjectPool.Instance.AddToPool(key, obj.gameObject);
        obj.GetComponent<ConveyorItem>().RemoveLink();
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
                            if(resourcesInTheMachine[item.Key] < 0) return;
                            go = ObjectPool.Instance.GrabFromPool("BoiledCorn", Library.Instance.boiledCorn);
                            resourcesInTheMachine[item.Key] -= 1;
                        break;

                        default:
                            return;
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
