using System.Security.AccessControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using MyEnums;

public class ResourceManager : Singleton<ResourceManager>
{
    #region Soil
    [Header("Soil")]
    public TextMeshProUGUI soilCounter;
    float _soil;

    public float SoilAmount()
    {
        return _soil;
    }

    public void IncrementSoil(float amount)
    {
        _soil += amount;
        UpdateSoiltext();
    }

    public void DecrementSoil(float amount)
    {
        _soil -= amount;
        UpdateSoiltext();
    }

    private void UpdateSoiltext()
    {
        soilCounter.text = "$" + _soil.ToString();
    }

    #endregion

    public bool HasResouce(ResourceType t, float amount)
    {
        switch(t)
        {
            case ResourceType.soil:
                return _soil >= amount;
        }

        return false;
    }

    public void SpendResouce(ResourceType t, float amount)
    {
        switch(t)
        {
            case ResourceType.soil:
                DecrementSoil(amount);
                break;
        }
    }
}
