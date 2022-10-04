using System.Security.AccessControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyEnums;

///<summary>
/// Create soil based on the input it recives
///</summary>
public class SoilMaker : Machine
{
    [Header("Soil Maker")]
    public ResourceType type;
    public GameObject outputPrefab;
    GameObject go;
    protected float resourceAmount;
    public float timeToExtract;
    float refTimer;

    private void Update() 
    {
        refTimer -= Time.deltaTime;
        
        if(refTimer <= 0) { 
            if(!isConnected) CheckOutput();
            OutputResource(); refTimer = timeToExtract; 
        }
    }

    public void OutputResource()
    {
        if(resourceAmount <= 0) return;

        if(outputPrefab != null)

        if(!isConnected) CheckOutput();
        if(!isConnected) return;

        if(type == ResourceType.none)
        {
            Debug.Log("NO RESOURCE SELECTED");
            return;
        }

        switch (type)
        {
            case ResourceType.soil:
                go = ObjectPool.Instance.GrabFromPool("Soil", Library.Instance.soilPrefab);
            break;
            case ResourceType.ore:
                go = ObjectPool.Instance.GrabFromPool("Ore", Library.Instance.soilPrefab);
            break;
            case ResourceType.stone:
                go = ObjectPool.Instance.GrabFromPool("Stone", Library.Instance.soilPrefab);
            break;
        }
        
        go.transform.position = outputPoint.transform.position;
        go.transform.rotation = Quaternion.identity;
        go.SetActive(true);

        resourceAmount--;
    }


    // Handle objects entering the machine
    private void OnTriggerStay(Collider other) 
    {
        if(other.gameObject.CompareTag("Ore"))
        {
            if(other.gameObject.GetComponent<ConveyorItem>().isLinked == false)
            {
                ObjectPool.Instance.AddToPool("Ore", other.gameObject);
                other.gameObject.SetActive(false);
                resourceAmount++;
            }
        }
        if(other.gameObject.CompareTag("Soil"))
        {
            if(other.gameObject.GetComponent<ConveyorItem>().isLinked == false)
            {
                ObjectPool.Instance.AddToPool("Soil", other.gameObject);
                other.gameObject.SetActive(false);
                resourceAmount++;
            }
        }
        if(other.gameObject.CompareTag("Stone"))
        {
            if(other.gameObject.GetComponent<ConveyorItem>().isLinked == false)
            {
                ObjectPool.Instance.AddToPool("Stone", other.gameObject);
                other.gameObject.SetActive(false);
                resourceAmount++;
            }
        }
    }
}
