using System.Security.AccessControl;
using System.Diagnostics.Contracts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyEnums;

public class BuildPrice : MonoBehaviour
{
    [NonReorderable]
    public List<Price> prices = new List<Price>();

    public bool CanPay()
    {
        // if any evaluete false, retun false
        foreach (Price item in prices)
        {
            if(ResourceManager.Instance.HasResouce(item.type, item.amount) == false)
                return false;
        }

        foreach (Price item in prices)
        {
            ResourceManager.Instance.SpendResouce(item.type, item.amount);
        }

        return true;
    }
}

[System.Serializable]
public class Price
{
    [SerializeField]
    public ResourceType type;
    public float amount;
}
