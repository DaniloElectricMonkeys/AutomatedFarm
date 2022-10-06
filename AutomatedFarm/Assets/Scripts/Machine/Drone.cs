using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyEnums;
using DG.Tweening;

public class Drone : PlantGraber
{
    [Header("Drone Attributes")]
    public bool collectBulk;
    public float droneSpeed;
    public GameObject droneObject;
    public ResourceType resourceToPick;
    public Transform deployPoint;
    bool isCollecting;
    float timeToGetThere;
    Vector3 endPoint;
    Vector3 midPoint;
    Vector3 distance;
    public int cahcedResources;


    void FlyThemCollect(GameObject obj)
    {
        endPoint = new Vector3(obj.transform.position.x, obj.transform.position.y+0.5f, obj.transform.position.z);
        midPoint = new Vector3(endPoint.x/2, 3, endPoint.z/2);
        distance = (endPoint - droneObject.transform.position);

        timeToGetThere = distance.magnitude * droneSpeed;

        droneObject.transform.DOMove(midPoint, timeToGetThere/2).SetEase(Ease.InCubic).OnComplete(FlyToEndPoint);
    }

    void FlyToEndPoint()
    {
        if(collectBulk)
            droneObject.transform.DOMove(endPoint, timeToGetThere/2).SetEase(Ease.OutCubic).OnComplete(FlyThemCollectNext);
        else
        {
            droneObject.transform.DOMove(endPoint, timeToGetThere/2).SetEase(Ease.OutCubic).OnComplete(FlyToMiddlePoint);
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
        droneObject.transform.DOMove(midPoint, timeToGetThere/2).SetEase(Ease.InCubic).OnComplete(FlyToDeployPoint);
    }

    void FlyToDeployPoint()
    {
        Vector3 endPoint = new Vector3(deployPoint.transform.position.x, deployPoint.transform.position.y+0.5f, deployPoint.transform.position.z);
        droneObject.transform.DOMove(endPoint, timeToGetThere).SetEase(Ease.OutCubic).OnComplete(FillBase);
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
        FlyThemCollect(cachedPlants.First().gameObject);
    }
}
