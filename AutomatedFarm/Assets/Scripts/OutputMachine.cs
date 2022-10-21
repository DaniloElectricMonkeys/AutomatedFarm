using System;
using System.Collections.Generic;
using UnityEngine;
using MyEnums;

///<summary>
/// Create soil based on the input it recives
///</summary>
public class OutputMachine : Machine
{
    [Header("Output Resource")]
    public int inventoryCapacity;
    public ResourceType outputType;
    protected GameObject go;
    [SerializeField] protected int resourceAmount;
    public float timeToExtract;
    protected float refTimer;

    [Space]
    [Header("Machine Door")]
    public Animator[] doorAnimator;

    protected Dictionary<string, int> resourcesInTheMachine = new Dictionary<string, int>();

    [Space]
    [Header("Crafting")]
    public List<ResourceType> typesNeededToCraft = new List<ResourceType>();

    [Space]
    [Header("Resource injection")]
    public List<ResourceToInject> resourcesToInject = new List<ResourceToInject>();

    public VFX_AnimationHandler handler;

    bool open;
    bool closed;

    private void Start() {
        refTimer = timeToExtract;
        RunOutput();
        UpdateDoorState();
    }

    public bool IsInventoryFull(ResourceType type = ResourceType.none, int doorNumber = -1)
    {
        var key = type.ToString();
        if (resourcesInTheMachine.ContainsKey(key))
        {
            if (resourcesInTheMachine[key] >= inventoryCapacity)
            {
                if (doorNumber != -1) doorAnimator[doorNumber].Play("Close");
                return true;
            }
        }
        else
        {
            if (doorNumber != -1) doorAnimator[doorNumber].Play("Open");
            return false;
        }
        
        if (doorNumber != -1) doorAnimator[doorNumber].Play("Open");
        return false;
    }

    private void UpdateDoorState()
    {
        if (resourceAmount >= inventoryCapacity && closed == false)
        {
            foreach (var item in doorAnimator)
                item.Play("Close");

            closed = true;
            open = false;
        }
        else if (open == false && resourceAmount < inventoryCapacity)
        {
            foreach (var item in doorAnimator)
                item.Play("Open");

            open = true;
            closed = false;
        }
    }

    public override void OnResourceEnter(ResourceType type, GameObject obj, int amout = 0)
    {
        string key = type.ToString();

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
            ObjectPool.Instance.AddToPool(key, obj.gameObject);
            obj.SetActive(false);
        }
        
        resourceAmount += amout;
    }

    private void Update() 
    {
        // UpdateDoorState();
        if(resourceAmount > 0)
        {
            refTimer -= Time.deltaTime;
            handler?.ResumeMachine();
        }
        else
        {
            handler?.StopMachine();
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
        
        go.GetComponent<ConveyorItem>().FreshSpawnItem();
        go.transform.position = outputPoint.transform.position;
        go.transform.rotation = Quaternion.identity;
        go.SetActive(true);

        resourceAmount--;
    }

    public int GetResourceAmount()
    {
        return resourceAmount;
    }

    public TEST_BeltItem AskForBeltItem()
    {
        if(refTimer > 0)
            return null;
        refTimer = timeToExtract;

        if(resourceAmount <= 0) 
            return null;

        if(!isConnected) CheckOutput();
        if(!isConnected) 
            return null;

        // Check if the itens needed to craft are in the machine
        foreach(var item in typesNeededToCraft)
        {
            // Return if itens are not found
            if(!resourcesInTheMachine.ContainsKey(item.ToString())) {
                return null;
            }
            
            // In this point we know that all itens needed are inside the machine.
            // Now we check if we have the required amount (>0).

            // Return if we have less or equal than 0 itens
            if(resourcesInTheMachine[item.ToString()] <= 0) {
                return null;
            }
        }
        
        // Now we have all itens, and their quantity is bigger then 0
        // Lets create the output and remove itens from the machine
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
                            go = ObjectPool.Instance.GrabFromPool(type.ToString(), ItemLibrary.Instance.rawCorn);
                            resourcesInTheMachine[item.Key] -= 1;
                        break;
                        case ResourceType.boiledCorn:
                            go = ObjectPool.Instance.GrabFromPool(type.ToString(), ItemLibrary.Instance.boiledCorn);
                            resourcesInTheMachine[item.Key] -= 1;
                        break;
                        case ResourceType.smashedCorn:
                            go = ObjectPool.Instance.GrabFromPool(type.ToString(), ItemLibrary.Instance.smashedCorn);
                            resourcesInTheMachine[item.Key] -= 1;
                        break;
                        case ResourceType.cookedCorn:
                            go = ObjectPool.Instance.GrabFromPool(type.ToString(), ItemLibrary.Instance.cookedCorn);
                            resourcesInTheMachine[item.Key] -= 1;
                        break;
                        case ResourceType.crystalCorn:
                            go = ObjectPool.Instance.GrabFromPool(type.ToString(), ItemLibrary.Instance.crystalCorn);
                            resourcesInTheMachine[item.Key] -= 1;
                        break;
                        case ResourceType.packedCorn:
                            go = ObjectPool.Instance.GrabFromPool(type.ToString(), ItemLibrary.Instance.packedCorn);
                            resourcesInTheMachine[item.Key] -= 1;
                        break;
                        case ResourceType.soil:
                            go = ObjectPool.Instance.GrabFromPool(type.ToString(), Library.Instance.soilPrefab);
                            resourcesInTheMachine[item.Key] -= 1;
                        break;
                        case ResourceType.ore:
                            go = ObjectPool.Instance.GrabFromPool(type.ToString(), Library.Instance.orePrefab);
                            resourcesInTheMachine[item.Key] -= 1;
                        break;
                        case ResourceType.stone:
                            go = ObjectPool.Instance.GrabFromPool(type.ToString(), Library.Instance.stonePrefab);
                            resourcesInTheMachine[item.Key] -= 1;
                        break;
                        case ResourceType.sugar:
                            go = ObjectPool.Instance.GrabFromPool(type.ToString(), ItemLibrary.Instance.sugar);
                            resourcesInTheMachine[item.Key] -= 1;
                        break;
                        case ResourceType.cardboard:
                            go = ObjectPool.Instance.GrabFromPool(type.ToString(), ItemLibrary.Instance.cardboard);
                            resourcesInTheMachine[item.Key] -= 1;
                        break;

                        default:
                            return null;
                    }
                    break;
                }
            }
            else
            {
                return null;
            }
        }
        else
        {
            // Create item
            go = ObjectPool.Instance.GrabFromPool(outputType.ToString(), ItemLibrary.Instance.GetPrefabFromType(outputType));   
        }

        // Remove from machine
        foreach(var item in typesNeededToCraft) {
            resourcesInTheMachine[item.ToString()]--;
        }
        
        go.GetComponent<ConveyorItem>()?.FreshSpawnItem();
        go.transform.position = outputPoint.transform.position;
        go.transform.rotation = Quaternion.identity;
        go.SetActive(true);

        resourceAmount--;
        FeedbackTextManager.Instance.SpawnText("+", transform.position + new Vector3(0,4,0));
        ResourceManager.Instance.IncrementSoil(1);
        return go.GetComponent<TEST_BeltItem>();
    }
}

[Serializable]
public class ResourceToInject
{
    public ResourceType type;
    public int quantity;

}

