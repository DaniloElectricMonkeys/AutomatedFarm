using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
/// Sell or store resources.
///</summary>
public class Seller : MonoBehaviour
{
    private void OnTriggerStay(Collider other) 
    {
        if(other.gameObject.CompareTag("Ore"))
        {
            if(other.gameObject.GetComponent<ConveyorItem>().isLinked == false)
            {
                ObjectPool.Instance.AddToPool("Ore", other.gameObject);
                other.gameObject.SetActive(false);
                // gain Money here
            }
        }
        if(other.gameObject.CompareTag("Soil"))
        {
            if(other.gameObject.GetComponent<ConveyorItem>().isLinked == false)
            {
                ObjectPool.Instance.AddToPool("Soil", other.gameObject);
                other.gameObject.SetActive(false);
                ResourceManager.Instance.IncrementSoil(1);
            }
        }
        if(other.gameObject.CompareTag("Stone"))
        {
            if(other.gameObject.GetComponent<ConveyorItem>().isLinked == false)
            {
                ObjectPool.Instance.AddToPool("Stone", other.gameObject);
                other.gameObject.SetActive(false);
            }
        }
    }
}
