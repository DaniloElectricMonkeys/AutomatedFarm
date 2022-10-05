using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Machine : MonoBehaviour
{
    [Header("Options")]
    public bool useInput;
    public bool useOutput;

    [Space]
    [Header("OutputOptions")]
    public bool isConnected;
    public GameObject outputPoint;


    //Override in other classes to add functionality
    public virtual void OnSoilEnter(){}
    public virtual void OnOreEnter(){}
    public virtual void OnStoneEnter(){}
    

    ///<summary>
    /// Check if there is a conveyor below the output
    ///</summary>
    protected void CheckOutput()
    {
        if(useOutput == false) return;

        if(Physics.Raycast(outputPoint.transform.position, Vector3.down, out RaycastHit hit, 10f))
        {
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
    }


    #region Input

        private void OnTriggerStay(Collider other) 
        {
            if(useInput == false) return;

            if(other.gameObject.CompareTag("Ore"))
            {
                if(other.gameObject.GetComponent<ConveyorItem>().isLinked == false)
                {
                    ObjectPool.Instance.AddToPool("Ore", other.gameObject);
                    other.gameObject.SetActive(false);
                    OnOreEnter();
                }
            }
            if(other.gameObject.CompareTag("Soil"))
            {
                if(other.gameObject.GetComponent<ConveyorItem>().isLinked == false)
                {
                    ObjectPool.Instance.AddToPool("Soil", other.gameObject);
                    other.gameObject.SetActive(false);
                    OnSoilEnter();
                }
            }
            if(other.gameObject.CompareTag("Stone"))
            {
                if(other.gameObject.GetComponent<ConveyorItem>().isLinked == false)
                {
                    ObjectPool.Instance.AddToPool("Stone", other.gameObject);
                    other.gameObject.SetActive(false);
                    OnStoneEnter();
                }
            }
        }

    #endregion 
}
