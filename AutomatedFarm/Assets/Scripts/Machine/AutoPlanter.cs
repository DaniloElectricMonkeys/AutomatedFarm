using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using MyEnums;

namespace AutomatedFarm
{
    public class AutoPlanter : Machine
    {
        [Header("Attributes")]
        public float influenceArea;
        public List<Collider> soilsHit = new List<Collider>();
        public List<Collider> cachedSoils = new List<Collider>();
        public Vector3 boxSize = new Vector3(10,3,10);
        public LayerMask soilLayer;
        public float timeToPlant;
        float refTime;
        private void Awake() {
            refTime = timeToPlant;
        }

        private void Start() {
            AssignSoils();
        }

        protected virtual void AssignSoils()
        {
            soilsHit = Physics.OverlapBox(transform.position, boxSize / 2, Quaternion.identity, soilLayer).ToList();

            if (soilsHit.Count <= 0) return;

            if(cachedSoils.Count > 0)
                cachedSoils.Clear();
            //Remove plants that are arealdy assigned to a graber
            foreach (var item in soilsHit)
            {
                PlantGrow plant = item.GetComponent<PlantGrow>();
                    cachedSoils.Add(item);
            }
        }

        public void PlantCrops()
        {
            if(cachedSoils.Count <= 0)
            {
                Debug.LogWarning("No Soil Cahced");
                return;
            } 

            List<Collider> tempList = cachedSoils.Where(p => p.gameObject.GetComponent<S_SoilState>().currentState != SoilState.planted).ToList();

            if(tempList.Count > 0)
            {
                S_SoilState selectedSoil = tempList.First().gameObject.GetComponent<S_SoilState>();
                selectedSoil.PlantOnSoil();
            }
            // else
            //     AssignSoils();
        }

        private void Update() 
        {
            refTime -= Time.deltaTime;

            if(refTime <= 0 && cachedSoils.Count > 0)
            {
                refTime = timeToPlant;
                PlantCrops();
            }
        }

        private void OnDrawGizmos() {
            Gizmos.DrawWireCube(transform.position, boxSize);
        }
    }
}
