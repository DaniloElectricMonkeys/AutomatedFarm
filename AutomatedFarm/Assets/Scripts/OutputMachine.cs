using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyEnums;
using DG.Tweening;

///<summary>
/// Create soil based on the input it recives
///</summary>
public class OutputMachine : Machine
{
    [Header("Output Resource")]
    public ResourceType outputType;
    protected GameObject go;
    [SerializeField] protected float resourceAmount;
    public float timeToExtract;
    protected float refTimer;

    protected Dictionary<string, int> resourcesInTheMachine = new Dictionary<string, int>();
    protected List<string> removeKeys = new List<string>();
    protected ConveyorItem item;
    public List<ResourceType> typesNeededToCraft = new List<ResourceType>();

    private void Start() {
        RunOutput();
    }

    public override void OnResourceEnter(ResourceType type, GameObject obj, int amout = 0)
    {
        string key = type.ToString();

        if(obj != null)
            item = obj.GetComponent<ConveyorItem>();
        if(item != null && item.dontKill) return;

        //Add object to the list of its type
        if(resourcesInTheMachine.ContainsKey(key))
            resourcesInTheMachine[key] += 1;
        //Add object to the list of its type by creating a new list if it is a new resource type
        else
        {
            int q = 1;
            resourcesInTheMachine.Add(key, q);
        }

        if(obj != null)
        {
            // obj.GetComponent<ConveyorItem>().RemoveLink();
            obj.transform.DOMove(new Vector3(transform.position.x, obj.transform.position.y, transform.position.z), timeToExtract)
            .SetEase(Ease.Linear)
            .OnComplete( () =>{
                ObjectPool.Instance.AddToPool(key, obj.gameObject);
                obj.SetActive(false);
            });
            // obj.SetActive(false);
        }
        
        resourceAmount += amout;
    }

    private void Update() 
    {
        if(resourceAmount <= 0){
            refTimer = timeToExtract;
            return;
        }
        
        refTimer -= Time.deltaTime;
        
        if(refTimer <= 0) { 
            if(!isConnected) CheckOutput();
            OutputResource(); refTimer = timeToExtract; 
        }
    }

    void RunOutput()
    {
        // Grab the iten to be crafted and populate the itens needed list. 
        // We cache the itens needed to craft for later use.
        foreach (var item in Library.Instance.itensSO.Itens)
        {
            if(item.craftableItem == outputType) {
                typesNeededToCraft.Clear();
                foreach (var types in item.itensNeededToCraft)
                    typesNeededToCraft.Add(types);

                break;
            }
        }

        // foreach(var item in typesNeededToCraft)
        //     if(!resourcesInTheMachine.ContainsKey(item.ToString()))
        //         Debug.LogError($"No item of {item.ToString()} found.");

    }

    public virtual void OutputResource()
    {
        if(resourceAmount <= 0) return;

        if(!isConnected) CheckOutput();
        if(!isConnected) return;

        // Check if the itens needed to craft are in the machine
        foreach(var item in typesNeededToCraft)
        {
            // Return if itens are not found
            if(!resourcesInTheMachine.ContainsKey(item.ToString())) {
                return;
            }
            
            // In this point we know that all itens needed are inside the machine.
            // Now we check if we have the required amount (>0).

            // Return if we have less or equal than 0 itens
            if(resourcesInTheMachine[item.ToString()] <= 0) {
                return;
            }
        }
        
        // Now we have all itens, and their quantity is bigger then 0
        // Lets create the output and remove itens from the machine

        // Create item
        go = ObjectPool.Instance.GrabFromPool(outputType.ToString(), ItemLibrary.Instance.GetPrefabFromType(outputType));

        // Remove from machine
        foreach(var item in typesNeededToCraft) {
            resourcesInTheMachine[item.ToString()]--;
        }

        // if(outputType == ResourceType.none)
        // {
        //     Debug.Log("NO RESOURCE SELECTED");
        //     return;
        // }

        // foreach(var item in resourcesInTheMachine)
        //     if(resourcesInTheMachine[item.Key] <= 0) 
        //         removeKeys.Add(item.Key);

        // foreach (var item in removeKeys)
        //     resourcesInTheMachine.Remove(item);

        // removeKeys.Clear();
        
        // switch (outputType)
        // {
        //     case ResourceType.soil:
        //         go = ObjectPool.Instance.GrabFromPool(outputType.ToString(), Library.Instance.soilPrefab);
        //     break;
        //     case ResourceType.ore:
        //         go = ObjectPool.Instance.GrabFromPool(outputType.ToString(), Library.Instance.soilPrefab);
        //     break;
        //     case ResourceType.stone:
        //         go = ObjectPool.Instance.GrabFromPool(outputType.ToString(), Library.Instance.soilPrefab);
        //     break;
        //     case ResourceType.corn:
        //         go = ObjectPool.Instance.GrabFromPool(outputType.ToString(), Library.Instance.rawCorn);
        //     break;
        //     case ResourceType.boiledCorn:
        //         go = ObjectPool.Instance.GrabFromPool(outputType.ToString(), Library.Instance.boiledCorn);
        //     break;
        //     case ResourceType.smashedCorn:
        //         go = ObjectPool.Instance.GrabFromPool(outputType.ToString(), Library.Instance.smashedCorn);
        //     break;
        //     case ResourceType.crystalCorn:
        //         go = ObjectPool.Instance.GrabFromPool(outputType.ToString(), Library.Instance.crystalCorn);
        //     break;
        // }
        
        go.GetComponent<ConveyorItem>().FreshSpawnItem();
        go.transform.position = outputPoint.transform.position;
        go.transform.rotation = Quaternion.identity;
        go.SetActive(true);

        resourceAmount--;
    }
}

