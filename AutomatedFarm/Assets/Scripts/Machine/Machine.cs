using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyEnums;

public class Machine : MonoBehaviour
{
    [Header("Options")]
    public bool useInput;
    public bool useOutput;

    [Space]
    [Header("OutputOptions")]
    public bool isConnected;
    public GameObject outputPoint;
    public GameObject checkConnectionPoint;

    private void Awake() {
        Conveyor.OnConveyorDeleted += CheckOutput;
    }
    //Override in other classes to add functionality
    public virtual void OnSoilEnter(){}
    public virtual void OnOreEnter(){}
    public virtual void OnStoneEnter(){}
    public virtual void OnResourceEnter(ResourceType type, GameObject obj)
    {
        string key = type.ToString();

        if(obj == null) return;
        ConveyorItem item = obj.GetComponent<ConveyorItem>();
        
        if(item.dontKill) return;
        
        ObjectPool.Instance.AddToPool(key, obj.gameObject);
        // item.RemoveLink();
        obj.SetActive(false);
    }


    ///<summary>
    /// Check if there is a conveyor below the output
    ///</summary>
    protected void CheckOutput()
    {
        if(useOutput == false) return;

        if(Physics.Raycast(checkConnectionPoint.transform.position, Vector3.down, out RaycastHit hit, 10f))
        {
            if(!hit.collider.gameObject.CompareTag("Conveyor"))
            {
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
            Debug.Log("NO OUTPUT HIT + " + gameObject.name);
        }
    }

    #region Input

        private void OnTriggerStay(Collider other) 
        {
            if(useInput == false) return;
            
            ConveyorItem item = other.GetComponent<ConveyorItem>();
            if(item == null) return;
            if(item.dontKill)
                item.transform.position += outputPoint.transform.forward * 1 * Time.deltaTime;
            else if(item != null)
                OnResourceEnter(item.type, other.gameObject);
        }

    #endregion 

    private void OnDestroy() {
        Conveyor.OnConveyorDeleted -= CheckOutput;
    }
}
