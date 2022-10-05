using System.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantGraber : OutputMachine
{
    [Header("Attributes")]
    public float influenceArea = 2;
    public LayerMask plantLayerMask;
    List<Collider> plantsHit = new List<Collider>();
    List<Collider> cachedPlants = new List<Collider>();

    private void Awake() {
        PlantGrow.OnPlantReady += CollectPlant;
        PlantGrow.OnPlantPlaced += AssignPlants;
    }

    private void OnDestroy() {
        PlantGrow.OnPlantReady -= CollectPlant;
        PlantGrow.OnPlantPlaced -= AssignPlants;
    }

    private void Start() {
        AssignPlants();
    }

    private void CollectPlant(GameObject plant) {
        foreach (var item in cachedPlants)
            if(item.GetComponent<PlantGrow>()?.Harvest() == true)
                resourceAmount++;
    }

    void AssignPlants(){
        plantsHit = Physics.OverlapBox(transform.position, ((Vector3.one * influenceArea)/2),Quaternion.identity, plantLayerMask).ToList();

        if(plantsHit.Count <= 0) return;

        //Remove plants that are arealdy assigned to a graber
        foreach (var item in plantsHit)
        {
            PlantGrow plant = item.GetComponent<PlantGrow>();
            if(plant.isAssignedToGraber == false) cachedPlants.Add(item);
        }

        foreach (var item in cachedPlants)
            CollectPlant(item.gameObject);
    }

    private void OnDrawGizmos() {
        Gizmos.DrawWireCube(transform.position, Vector3.one * influenceArea);
    }
}
