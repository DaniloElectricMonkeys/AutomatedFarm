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
    public bool collectBulk;
    public GameObject droneObject;
    public ResourceType resourceToPick;
    public Transform deployPoint;
    bool isCollecting;
    public float totalTimeToTravel;
    Vector3 endPoint;
    Vector3 midPoint;
    Vector3 distance;
    public int cahcedResources;
    Vector3 plant;
    bool isFlying;
    float curveTimer;

    void FlyToCrop()//Step 01
    {
        plant = cachedPlants.First().transform.position;
        endPoint = new Vector3(plant.x, plant.y, plant.z);
        droneObject.transform.DOMoveX(endPoint.x, totalTimeToTravel).SetEase(Ease.InOutCubic);
        droneObject.transform.DOMoveZ(endPoint.z, totalTimeToTravel).SetEase(Ease.InOutCubic);
        isFlying = true;
    }

    private void Update() {
        if(!isFlying)
        {
            curveTimer = 0;
            return;
        }

        curveTimer += Time.deltaTime;
        droneObject.transform.position = new Vector3(droneObject.transform.position.x, yCurve.Evaluate(curveTimer/totalTimeToTravel), droneObject.transform.position.z);
    }

    void FlyThemCollect(GameObject obj)
    {
        endPoint = new Vector3(obj.transform.position.x, obj.transform.position.y+0.5f, obj.transform.position.z);
        midPoint = new Vector3(endPoint.x/2, 3, endPoint.z/2);
        distance = (endPoint - droneObject.transform.position);


        droneObject.transform.DOMove(midPoint, totalTimeToTravel/2).SetEase(Ease.InCubic).OnComplete(FlyToEndPoint);
    }

    void FlyToEndPoint()
    {
        if(collectBulk)
            droneObject.transform.DOMove(endPoint, totalTimeToTravel/2).SetEase(Ease.OutCubic).OnComplete(FlyThemCollectNext);
        else
        {
            droneObject.transform.DOMove(endPoint, totalTimeToTravel/2).SetEase(Ease.OutCubic).OnComplete(FlyToMiddlePoint);
            cachedPlants.RemoveAt(0);
        }
    }

    void FlyThemCollectNext()
    {   
        cachedPlants.RemoveAt(0);
        cahcedResources++;

        if(cachedPlants.Count <= 0)
            FlyToMiddlePoint();
        else
            FlyThemCollect(cachedPlants.First().gameObject);
    }

    void FlyToMiddlePoint()
    {
        droneObject.transform.DOMove(midPoint, totalTimeToTravel/2).SetEase(Ease.InCubic).OnComplete(FlyToDeployPoint);
    }

    void FlyToDeployPoint()
    {
        Vector3 endPoint = new Vector3(deployPoint.transform.position.x, deployPoint.transform.position.y+0.5f, deployPoint.transform.position.z);
        droneObject.transform.DOMove(endPoint, totalTimeToTravel).SetEase(Ease.OutCubic).OnComplete(FillBase);
    }

    void FillBase()
    {
        if(collectBulk)
        {
            resourceAmount += cahcedResources;
            cahcedResources = 0;
        }
        else
        {
            resourceAmount++;
            cachedPlants.RemoveAt(0);
            if(cachedPlants.Count > 0)
                FlyThemCollect(cachedPlants.First().gameObject);
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
    protected override void CollectOnAssign()
    {
        isCollecting = true;
        // FlyThemCollect(cachedPlants.First().gameObject);
        FlyToCrop();
    }
}
