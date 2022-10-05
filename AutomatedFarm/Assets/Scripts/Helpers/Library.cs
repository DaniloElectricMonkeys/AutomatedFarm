using System.Collections.Generic;
using UnityEngine;

///<summary>
/// Hold all the info about buildings and prefabs;
///</summary>
public class Library : Singleton<Library>
{
    [Header("Dependecys")]
    public SO_Buildings buildingsSO;

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
}