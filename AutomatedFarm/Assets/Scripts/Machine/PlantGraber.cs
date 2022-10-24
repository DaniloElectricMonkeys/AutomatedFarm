using System.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AutomatedFarm
{
    public class PlantGraber : OutputMachine
    {
        [Space]
        [Header("Collection Area")]
        public Vector3 boxSize = new Vector3(10,3,10);
        public Vector3 boxOffset;
        public LayerMask plantLayerMask;
        List<Collider> plantsHit = new List<Collider>();
        protected List<Collider> cachedPlants = new List<Collider>();

        protected virtual void Awake() {
            PlantGrow.OnPlantReady += CollectPlant;
            PlantGrow.OnPlantPlaced += AssignPlants;
        }

        protected virtual void OnDestroy() {
            PlantGrow.OnPlantReady -= CollectPlant;
            PlantGrow.OnPlantPlaced -= AssignPlants;
        }

        private void Start() {
            AssignPlants();
        }

        protected virtual void CollectPlant(GameObject plant) {
            foreach (var item in cachedPlants)
                if(item.GetComponent<PlantGrow>()?.Harvest() == true)
                    resourceAmount++;
        }

        protected virtual void AssignPlants()
        {
            plantsHit = Physics.OverlapBox(transform.position, boxSize / 2, Quaternion.identity, plantLayerMask).ToList();

            if (plantsHit.Count <= 0) return;

            if(cachedPlants.Count > 0)
                cachedPlants.Clear();
            //Remove plants that are arealdy assigned to a graber
            foreach (var item in plantsHit)
            {
                PlantGrow plant = item.GetComponent<PlantGrow>();
                    cachedPlants.Add(item);
            }

            AskForCollection();
        }

        protected virtual void AskForCollection()
        {
            foreach (var item in cachedPlants)
                CollectPlant(item.gameObject);
        }

        private void OnDrawGizmos() {
            Gizmos.DrawWireCube(transform.position, boxSize);
        }
    }

}

