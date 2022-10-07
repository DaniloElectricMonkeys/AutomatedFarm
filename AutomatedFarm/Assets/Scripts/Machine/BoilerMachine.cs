using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyEnums;
using System;

public class BoilerMachine : OutputMachine
{
    public override void OutputResource()
    {
        //if(resourceAmount <= 0) return;

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
                            go = ObjectPool.Instance.GrabFromPool(ResourceType.boiledCorn.ToString(), Library.Instance.boiledCorn);
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
            
        go.transform.position = outputPoint.transform.position;
        go.transform.rotation = Quaternion.identity;
        go.SetActive(true);
        go.GetComponent<ConveyorItem>().FreshSpawnItem();

        resourceAmount--;
    }
}
