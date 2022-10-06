using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyEnums;
using DG.Tweening;

public class Drone : PlantGraber
{
    [Header("Drone Attributes - Curve")]
    public AnimationCurve yCurve;

    [Header("Drone Attributes")]
    public float speed;
    public bool collectBulk;
    public GameObject droneObject;
    public GameObject armObject;
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

    private void Awake() {
        PlantGrow.OnPlantReady += CanCollect;
    }
    void FlyToCrop()//Step 01
    {
        if(readyPlants.Count <= 0)
        {
            FlyToDeployPoint();
            return;
        }

        plant = readyPlants.First().transform.position;
        endPoint = new Vector3(plant.x, plant.y, plant.z);
        distance = (endPoint - droneObject.transform.position);
        timeToTravel = distance.magnitude / speed;

        droneObject.transform.DOMoveZ(endPoint.z, timeToTravel).SetEase(Ease.InOutCubic).OnComplete(HarvestPlant);
        anyTween = droneObject.transform.DOMoveX(endPoint.x, timeToTravel).SetEase(Ease.InOutCubic).OnUpdate( () => 
        {
            //move the y based on the curve
            armObject.transform.LookAt(plant);
            Vector3 nextPoint = new Vector3(droneObject.transform.position.x, yCurve.Evaluate(anyTween.Elapsed() / timeToTravel), droneObject.transform.position.z);
            droneObject.transform.position = nextPoint;
        });
    }

    void HarvestPlant()//Setp 02
    {
        //Get plant reference and remove it from soil
        readyPlants.RemoveAt(0);
        cahcedResources++;

        if(collectBulk)
            FlyToCrop();
        else
            FlyToDeployPoint();
    }

    void FlyToDeployPoint()//Step 03
    {
        endPoint = new Vector3(deployPoint.transform.position.x, deployPoint.transform.position.y, deployPoint.transform.position.z);
        distance = (endPoint - droneObject.transform.position);
        timeToTravel = distance.magnitude / speed;

        droneObject.transform.DOMoveX(endPoint.x, timeToTravel).SetEase(Ease.InOutCubic).OnComplete(FillBase);
        anyTween = droneObject.transform.DOMoveZ(endPoint.z, timeToTravel).SetEase(Ease.InOutCubic).OnUpdate(() => {
            //move the y based on the curve
            armObject.transform.LookAt(deployPoint);
            Vector3 nextPoint = new Vector3(droneObject.transform.position.x, yCurve.Evaluate(anyTween.Elapsed() / timeToTravel), droneObject.transform.position.z);
            droneObject.transform.position = nextPoint;
        });
    }

    void FillBase()//Step 04
    {
        resourceAmount += cahcedResources;
        cahcedResources = 0;

        if(readyPlants.Count > 0)
            FlyToCrop();
        else
        {
            isCollecting = false;
            AskForCollection();
        }
    }

    protected override void CollectPlant(GameObject plant)
    {
        //Do nothing here
    }

    protected override void AssignPlants()
    {
        if(isCollecting) return;
        base.AssignPlants();
    }

    protected override void AskForCollection()
    {
        CanCollect();
    }

    private void CanCollect(GameObject obj = null)
    {
        if (isCollecting) return;
        // FlyThemCollect(cachedPlants.First().gameObject);
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
}
