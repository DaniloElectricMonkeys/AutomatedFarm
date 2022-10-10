using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyEnums;
using System;

public class Smasher : OutputMachine
{
    public override void OnResourceEnter(ResourceType type, GameObject obj, int amout)
    {
        base.OnResourceEnter(type, obj, 1);
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

        foreach(var item in resourcesInTheMachine)
            if(resourcesInTheMachine[item.Key] <= 0) 
                removeKeys.Add(item.Key);

        foreach (var item in removeKeys)
            resourcesInTheMachine.Remove(item);

        removeKeys.Clear();

        if(outputType == ResourceType.variable)
        {
            if(resourcesInTheMachine.Count > 0)
            {
                foreach(var item in resourcesInTheMachine)
                {
                    ResourceType type = (ResourceType)Enum.Parse(typeof(ResourceType), item.Key);

                    switch (type)
                    {
                        case ResourceType.boiledCorn:
                            go = ObjectPool.Instance.GrabFromPool("smashedCorn", Library.Instance.smashedCorn);
                            resourcesInTheMachine[item.Key] -= 1;
                        break;

                        default:
                            return;
                    }
                    break;
                }
            }
            else
                return;
        }
        else
        {
            Debug.LogError("NO RESOURCE SELECTED - BOILING MACHINE");
            return;
        }

        go.GetComponent<ConveyorItem>().dontKill = true;
        go.transform.position = outputPoint.transform.position;
        go.transform.rotation = Quaternion.identity;
        go.SetActive(true);

        resourceAmount--;
    }
}
