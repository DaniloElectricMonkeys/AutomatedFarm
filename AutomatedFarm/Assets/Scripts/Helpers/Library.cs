using System.Collections.Generic;
using UnityEngine;

///<summary>
/// Hold all the info about buildings and prefabs;
///</summary>
public class Library : Singleton<Library>
{
    [Header("ResourcePrefabs")]
    public GameObject soilPrefab;
    public GameObject orePrefab;
    public GameObject stonePrefab;

    [Header("Building")]
    public GameObject currentSelected;
    public GameObject conveyor;
    public GameObject B_conveyor;
    public GameObject singleMiner;
    public GameObject B_singleMiner;
    public GameObject seller;
    public GameObject B_seller;
    public GameObject fertileSoil;
    public GameObject B_fertileSoil;
    public GameObject plantGraber;
    public GameObject B_plantGraber;


    // Those are called by UI Buttons
    public void PickConveyor()
    {
        BuildSystem.Instance.ChosseObject(B_conveyor);
        currentSelected = conveyor;
    }
    public void PickSeller()
    {
        BuildSystem.Instance.ChosseObject(B_seller);
        currentSelected = seller;
    }
    public void PickRockSmasher()
    {
        BuildSystem.Instance.ChosseObject(B_singleMiner);
        currentSelected = singleMiner;
    }

    public void PickFertileSoil()
    {
        BuildSystem.Instance.ChosseObject(B_fertileSoil);
        currentSelected = fertileSoil;
    }

    public void PickPlantGraber()
    {
        BuildSystem.Instance.ChosseObject(B_plantGraber);
        currentSelected = plantGraber;
    }
}