using UnityEngine;
using MyEnums;

///<summary>
/// Hold all the info about buildings and prefabs;
///</summary>
public class Library : Singleton<Library>
{
    [Header("Dependecys")]
    public SO_Buildings buildingsSO;
    public SeedSO[] seedScriptable;

    [Space]
    [Header("Corn Prefabs")]
    public GameObject rawCorn;
    public GameObject boiledCorn;

    [Space]
    [Header("Resource Prefabs")]
    public GameObject soilPrefab;
    public GameObject orePrefab;
    public GameObject stonePrefab;

    [Header("Building")]
    public GameObject currentSelected;
    
    [Header("UI")]
    public ToolTip toolTip;


    public void ChooseBuilding(int ID)
    {
        BuildSystem.Instance.ChosseObject(buildingsSO.buildings[ID].blueprint);
        currentSelected = buildingsSO.buildings[ID].original;
    }

    public void SetCurrentSelected(GameObject obj)
    {
        currentSelected = obj;
    }

    public Seed GetType(ResourceType resource)
    {
        for (int i = 0; i < seedScriptable[0].seeds.Length; i++)
        {
            if (resource == seedScriptable[0].seeds[i].type) return seedScriptable[0].seeds[i];
        }

        return null;
    }
}