using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
/// Create soil based on the input it recives
///</summary>
public class SoilMaker : SingleMiner
{
    [Header("Soil Maker")]
    public GameObject outputPrefab;
    GameObject go;
    float stoneAmount;

    public override void Start() 
    {
        base.Start();
        OnSpawn += CreateSoil;
    }

    public void CreateSoil()
    {
        if(stoneAmount <= 0) return;

        if(outputPrefab != null)

        go = ObjectPool.Instance.GrabFromPool("Soil", Library.Instance.soilPrefab);
        
        go.transform.position = outputPoint.transform.position;
        go.transform.rotation = Quaternion.identity;
        go.SetActive(true);

        stoneAmount--;
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
            }
        }
        if(other.gameObject.CompareTag("Soil"))
        {
            if(other.gameObject.GetComponent<ConveyorItem>().isLinked == false)
            {
                ObjectPool.Instance.AddToPool("Soil", other.gameObject);
                other.gameObject.SetActive(false);
            }
        }
        if(other.gameObject.CompareTag("Stone"))
        {
            if(other.gameObject.GetComponent<ConveyorItem>().isLinked == false)
            {
                ObjectPool.Instance.AddToPool("Stone", other.gameObject);
                other.gameObject.SetActive(false);
                stoneAmount++;
            }
        }
    }
}
