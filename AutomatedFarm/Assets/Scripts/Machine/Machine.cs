using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyEnums;

namespace AutomatedFarm
{
    public class Machine : MonoBehaviour
    {
        [Header("Options")]
        public bool useInput;
        public bool useOutput;
        public LayerMask machineLayer;

        [Space]
        [Header("OutputOptions")]
        public bool isConnected;
        public GameObject outputPoint;
        public GameObject checkConnectionPoint;

        //Override in other classes to add functionality
        public virtual void OnSoilEnter(){}
        public virtual void OnOreEnter(){}
        public virtual void OnStoneEnter(){}
        public virtual void OnResourceEnter(ResourceType type, GameObject obj, int amout = 0)
        {
            string key = type.ToString();
            ObjectPool.Instance.AddToPool(key, obj.gameObject);
            obj.SetActive(false);
        }


        ///<summary>
        /// Check if there is a conveyor below the output
        ///</summary>
        protected void CheckOutput()
        {
            if(useOutput == false) return;

            if(Physics.Raycast(checkConnectionPoint.transform.position, Vector3.down, out RaycastHit hit, 10f, machineLayer))
            {
                if(!hit.collider.gameObject.CompareTag("Conveyor"))
                {
                    Debug.Log(hit.collider.gameObject.name);
                    isConnected = false;
                    return;
                }

                if(hit.collider.gameObject.CompareTag("Conveyor") && isConnected == false)
                {
                    isConnected = true;
                    SideCheck[] checkers = hit.collider.gameObject.GetComponentsInChildren<SideCheck>();
                    if(checkers == null) return;

                    foreach (SideCheck item in checkers)
                    {
                        item.CheckForMachineConnection();
                    }
                }
                else if(isConnected)
                    return;
                else
                    isConnected = false;
            }
            else
            {
                isConnected = false;
            }
        }
    }

}

