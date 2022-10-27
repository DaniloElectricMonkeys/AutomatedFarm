using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyEnums;
using DG.Tweening;
using System;

namespace AutomatedFarm
{
    public class Drone : PlantGraber
    {
        [Header("Drone Attributes - Curve")]
        public AnimationCurve yCurve;

        [Header("Drone Attributes")]
        public float speed;
        public bool collectBulk;
        public GameObject droneObject;
        public ResourceType resourceToPick;
        public Transform deployPoint;
        public bool isCollecting;
        float timeToTravel;
        Vector3 endPoint;
        Vector3 distance;
        public int cahcedResources;
        Vector3 plant;
        Tween anyTween;
        public List<GameObject> readyPlants = new List<GameObject>();
        bool doOnce;
        Quaternion q;
        public PlantGrow selectedPlant;
        bool _continue;
        int ID;

        private void Start() {
            AssignPlants();
            foreach (var item in resourcesToInject)
            {
                resourcesInTheMachine.Add(item.type.ToString(), item.quantity);
                resourceAmount += item.quantity;
            }
        }

        protected override void Awake() {
            PlantGrow.OnPlantReady += ThisAssign;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            PlantGrow.OnPlantReady -= ThisAssign;
        }

        void CheckPlantList(out bool canContinue)
        {
            readyPlants.Clear();
            List<Collider> removeItens = new List<Collider>();
            if(cachedPlants.Count <= 0){
                isCollecting = false;
                AssignPlants();
                canContinue = false;
                return;
            }

            foreach (var item in cachedPlants)
            {
                if(item == null)
                    removeItens.Add(item);
            }       

            foreach (var item in removeItens)
                cachedPlants.Remove(item);

            foreach (var item in cachedPlants)
                readyPlants.Add(item.gameObject);
            
            canContinue = true;
        }

        void FlyToCrop()//Step 01
        {
            doOnce = false;
            if(readyPlants.Count <= 0)
            {
                FlyToDeployPoint();
                return;
            }
            
            List<GameObject> tempList = readyPlants.Where(p => p != null).ToList();
            ID = UnityEngine.Random.Range(0,tempList.Count);
            selectedPlant = tempList[ID].GetComponent<PlantGrow>();

            if(selectedPlant == null || selectedPlant.istarget){
                CheckPlantList(out _continue);
            }

            if(selectedPlant != null && selectedPlant.istarget == false) selectedPlant.istarget = true;
            else
                CheckPlantList(out _continue);

            if(_continue == false)
            {
                isCollecting = false;
                AssignPlants();
                return;
            }
            
            plant = selectedPlant.transform.position;
            endPoint = new Vector3(plant.x, plant.y, plant.z);
            distance = (endPoint - droneObject.transform.position);
            timeToTravel = distance.magnitude / speed;

            q = Quaternion.LookRotation((plant - droneObject.transform.position) + new Vector3(0,-1.5f,0), Vector3.up);
            droneObject.transform.DORotateQuaternion(q, 1f).SetEase(Ease.InOutCubic);

            // droneObject.transform.DOMoveZ(endPoint.z, timeToTravel).SetEase(Ease.InOutCubic).OnComplete(HarvestPlant);
            // droneObject.transform.DOMoveX(endPoint.x, timeToTravel).SetEase(Ease.InOutCubic);
            anyTween = droneObject.transform.DOMove(endPoint, timeToTravel).SetEase(Ease.InOutCubic).OnComplete(HarvestPlant).OnUpdate( () => 
            {
                //move the y based on the curve
                // Vector3 nextPoint = new Vector3(droneObject.transform.position.x, yCurve.Evaluate((timeToTravel-anyTween.Elapsed()) / timeToTravel), droneObject.transform.position.z);
                // droneObject.transform.position = nextPoint;
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
            readyPlants.RemoveAt(ID);
            try{
                OnResourceEnter(selectedPlant.type, null, 0);
                selectedPlant.Harvest();
                cahcedResources++;
            }
            catch{
                //Ignore
            }
            

            if(collectBulk)
                FlyToCrop();
            else
                FlyToDeployPoint();
        }

        void FlyToDeployPoint()//Step 03
        {
            doOnce = false;
            endPoint = new Vector3(deployPoint.transform.position.x, (deployPoint.transform.position.y - 0.5f), deployPoint.transform.position.z);
            distance = (endPoint - droneObject.transform.position);
            timeToTravel = distance.magnitude / speed;

            q = Quaternion.LookRotation((deployPoint.position - plant) + new Vector3(0,-1.5f,0), Vector3.up);
            droneObject.transform.DORotateQuaternion(q, 1f).SetEase(Ease.InOutCubic);

            droneObject.transform.DOMoveX(endPoint.x, timeToTravel).SetEase(Ease.InOutCubic).OnComplete(FillBase);
            
            // droneObject.transform.DOMoveZ(endPoint.z, timeToTravel);
            anyTween = droneObject.transform.DOMove(endPoint, timeToTravel).SetEase(Ease.InOutCubic).OnUpdate(() => {
                //move the y based on the curve
                // Vector3 nextPoint = new Vector3(droneObject.transform.position.x, yCurve.Evaluate( Mathf.Clamp(anyTween.Elapsed(), 0.01f, 10) / timeToTravel), droneObject.transform.position.z);
                // droneObject.transform.position = nextPoint;
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
            resourceAmount += cahcedResources;
            cahcedResources = 0;

            CheckPlantList(out _continue);

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

        public override GameObject VariableItem()
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

                return go;
        }
        // public override void OutputResource()
        // {
        //     return;
        //     if(resourceAmount <= 0) return;

        //     if(!isConnected) CheckOutput();
        //     if(!isConnected) return;

        //     if(outputType == ResourceType.none)
        //     {
        //         Debug.Log("NO RESOURCE SELECTED");
        //         return;
        //     }

        //     foreach(var item in resourcesInTheMachine)
        //         if(resourcesInTheMachine[item.Key] <= 0) 
        //             removeKeys.Add(item.Key);

        //     foreach (var item in removeKeys)
        //         resourcesInTheMachine.Remove(item);

        //     removeKeys.Clear();

        //     if(outputType == ResourceType.variable)
        //     {
        //         if(resourcesInTheMachine.Count > 0)
        //         {
        //             foreach(var item in resourcesInTheMachine)
        //             {
        //                 ResourceType type = (ResourceType)Enum.Parse(typeof(ResourceType), item.Key);
                        
        //                 switch (type)
        //                 {
        //                     case ResourceType.corn:
        //                         go = ObjectPool.Instance.GrabFromPool(type.ToString(), ItemLibrary.Instance.rawCorn);
        //                         resourcesInTheMachine[item.Key] -= 1;
        //                     break;
        //                     case ResourceType.boiledCorn:
        //                         go = ObjectPool.Instance.GrabFromPool(type.ToString(), Library.Instance.boiledCorn);
        //                         resourcesInTheMachine[item.Key] -= 1;
        //                     break;
        //                     case ResourceType.smashedCorn:
        //                         go = ObjectPool.Instance.GrabFromPool(type.ToString(), Library.Instance.smashedCorn);
        //                         resourcesInTheMachine[item.Key] -= 1;
        //                     break;
        //                     case ResourceType.soil:
        //                         go = ObjectPool.Instance.GrabFromPool(type.ToString(), Library.Instance.soilPrefab);
        //                         resourcesInTheMachine[item.Key] -= 1;
        //                     break;
        //                     case ResourceType.ore:
        //                         go = ObjectPool.Instance.GrabFromPool(type.ToString(), Library.Instance.orePrefab);
        //                         resourcesInTheMachine[item.Key] -= 1;
        //                     break;
        //                     case ResourceType.stone:
        //                         go = ObjectPool.Instance.GrabFromPool(type.ToString(), Library.Instance.stonePrefab);
        //                         resourcesInTheMachine[item.Key] -= 1;
        //                     break;

        //                     default:
        //                         return;
        //                 }
        //                 break;
        //             }
        //         }
        //         else
        //             return;
        //     }
        //     else
        //     {
        //         Debug.LogError("NO RESOURCE SELECTED - BOILING MACHINE");
        //         return;
        //     }

        //     if(go.GetComponent<ConveyorItem>() != null)    
        //         go.GetComponent<ConveyorItem>().dontKill = true;

        //     go.transform.position = outputPoint.transform.position;
        //     go.transform.rotation = Quaternion.identity;
        //     go.SetActive(true);

        //     resourceAmount--;
        // }
    }
}