using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyEnums;
using DG.Tweening;
using System;

public class Drone : PlantGraber
{
    [Header("Drone Attributes - Curve")]
    public AnimationCurve yCurve;

    [Header("Drone Attributes")]
    public GameObject resourcePocket;
    public float speed;
    public bool collectBulk;
    public GameObject droneObject;
    public ResourceType resourceToPick;
    public Transform deployPoint;
    bool isCollecting;
    float timeToTravel;
    Vector3 endPoint;
    Vector3 distance;
    public int cahcedResources;
    Vector3 plant;
    Tween anyTween;
    List<GameObject> readyPlants = new List<GameObject>();
    PlantGrow currentPlant;
    bool doOnce;
    Quaternion q;

    private void Start() {
        AssignPlants();
    }

    private void Awake() {
        PlantGrow.OnPlantReady += ThisAssign;
    }
    void FlyToCrop()//Step 01
    {
        doOnce = false;
        if(readyPlants.Count <= 0)
        {
            FlyToDeployPoint();
            return;
        }

        plant = readyPlants.First().transform.position;
        endPoint = new Vector3(plant.x, plant.y, plant.z);
        distance = (endPoint - droneObject.transform.position);
        timeToTravel = distance.magnitude / speed;

        q = Quaternion.LookRotation((plant - droneObject.transform.position) + new Vector3(0,-1.5f,0), Vector3.up);
        droneObject.transform.DORotateQuaternion(q, 1f).SetEase(Ease.InOutCubic);

        droneObject.transform.DOMoveZ(endPoint.z, timeToTravel).SetEase(Ease.InOutCubic).OnComplete(HarvestPlant);
        anyTween = droneObject.transform.DOMoveX(endPoint.x, timeToTravel).SetEase(Ease.InOutCubic).OnUpdate( () => 
        {
            //move the y based on the curve
            Vector3 nextPoint = new Vector3(droneObject.transform.position.x, yCurve.Evaluate(anyTween.Elapsed() / timeToTravel), droneObject.transform.position.z);
            droneObject.transform.position = nextPoint;
            if(anyTween.Elapsed() >= (timeToTravel/2) && doOnce == false)
            {
                Vector3 newDirection = new Vector3(droneObject.transform.forward.x, 0, droneObject.transform.forward.z);
                q = Quaternion.LookRotation(newDirection, Vector3.up);
                droneObject.transform.DORotateQuaternion(q, 1f).SetEase(Ease.InOutCubic);
                doOnce = true;
            }
        });
    }

    void HarvestPlant()//Setp 02
    {
        //Get plant reference and remove it from soil
        resourcePocket.SetActive(true);
        currentPlant = readyPlants.First().GetComponent<PlantGrow>();
        cahcedResources++;
        Debug.Log(currentPlant.type.ToString());
        OnResourceEnter(currentPlant.type, null);

        readyPlants.RemoveAt(0);
        currentPlant.Harvest();

        if(collectBulk)
            FlyToCrop();
        else
            FlyToDeployPoint();
    }

    void FlyToDeployPoint()//Step 03
    {
        doOnce = false;
        endPoint = new Vector3(deployPoint.transform.position.x, deployPoint.transform.position.y, deployPoint.transform.position.z);
        distance = (endPoint - droneObject.transform.position);
        timeToTravel = distance.magnitude / speed;

        q = Quaternion.LookRotation((deployPoint.position - plant) + new Vector3(0,-1.5f,0), Vector3.up);
        droneObject.transform.DORotateQuaternion(q, 1f).SetEase(Ease.InOutCubic);

        droneObject.transform.DOMoveX(endPoint.x, timeToTravel).SetEase(Ease.InOutCubic).OnComplete(FillBase);
        anyTween = droneObject.transform.DOMoveZ(endPoint.z, timeToTravel).SetEase(Ease.InOutCubic).OnUpdate(() => {
            //move the y based on the curve
            Vector3 nextPoint = new Vector3(droneObject.transform.position.x, yCurve.Evaluate(anyTween.Elapsed() / timeToTravel), droneObject.transform.position.z);
            droneObject.transform.position = nextPoint;
            if(anyTween.Elapsed() >= (timeToTravel/2) && doOnce == false)
            {
                Vector3 newDirection = new Vector3(droneObject.transform.forward.x, 0, droneObject.transform.forward.z);
                q = Quaternion.LookRotation(newDirection, Vector3.up);
                droneObject.transform.DORotateQuaternion(q, 1f).SetEase(Ease.InOutCubic);
                doOnce = true;
            }
        });
    }

    void FillBase()//Step 04
    {
        

        resourcePocket.SetActive(false);
        resourceAmount += cahcedResources;
        cahcedResources = 0;

        if(readyPlants.Count > 0)
            FlyToCrop();
        else
        {
            isCollecting = false;
            ThisAssign();
        }
    }

    protected override void CollectPlant(GameObject plant)
    {
        //Do nothing here
    }

    void ThisAssign(GameObject obj = null)
    {
        if(isCollecting) return;
        AssignPlants();
    }

    protected override void AssignPlants()
    {
        if(isCollecting) return;
        base.AssignPlants();
    }

    protected override void AskForCollection()
    {
        if (isCollecting) return;
        foreach (var item in cachedPlants)
        {
            if (item.GetComponent<PlantGrow>().canBeHarvested)
                if (!readyPlants.Contains(item.gameObject))
                    readyPlants.Add(item.gameObject);
        }
        if (readyPlants.Count > 0) isCollecting = true;
        else return;
        FlyToCrop();
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
                        case ResourceType.corn:
                            go = ObjectPool.Instance.GrabFromPool(type.ToString(), Library.Instance.rawCorn);
                            resourcesInTheMachine[item.Key] -= 1;
                        break;
                        case ResourceType.boiledCorn:
                            go = ObjectPool.Instance.GrabFromPool(type.ToString(), Library.Instance.boiledCorn);
                            resourcesInTheMachine[item.Key] -= 1;
                        break;
                        case ResourceType.smashedCorn:
                            go = ObjectPool.Instance.GrabFromPool(type.ToString(), Library.Instance.smashedCorn);
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
