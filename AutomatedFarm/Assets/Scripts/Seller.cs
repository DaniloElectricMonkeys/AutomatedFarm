using System.Collections;
using System.Collections.Generic;
using MyEnums;
using UnityEngine;

///<summary>
/// Sell or store resources.
///</summary>
public class Seller : Machine
{
    public override void OnResourceEnter(ResourceType type, GameObject obj)
    {
        base.OnResourceEnter(type, obj);
        switch(type)
        {
            case ResourceType.corn:
                ResourceManager.Instance.IncrementSoil(1);
            break;
            case ResourceType.boiledCorn:
                ResourceManager.Instance.IncrementSoil(2);
            break;
            case ResourceType.smashedCorn:
                ResourceManager.Instance.IncrementSoil(3);
            break;  
            case ResourceType.cookedCorn:
                ResourceManager.Instance.IncrementSoil(4);
            break;
            case ResourceType.crystalCorn:
                ResourceManager.Instance.IncrementSoil(5);
            break;
            case ResourceType.packedCorn:
                ResourceManager.Instance.IncrementSoil(6);
            break;
            
        }
        
    }
}
