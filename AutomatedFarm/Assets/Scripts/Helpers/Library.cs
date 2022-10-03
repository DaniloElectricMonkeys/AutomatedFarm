using System.Collections.Generic;
using UnityEngine;

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

    public void PickConveyor()
    {
        BuildSystem._Instance.ChosseObject(B_conveyor);
        currentSelected = conveyor;
    }
    public void PickSeller()
    {
        BuildSystem._Instance.ChosseObject(B_seller);
        currentSelected = seller;
    }
    public void PickRockSmasher()
    {
        BuildSystem._Instance.ChosseObject(B_singleMiner);
        currentSelected = singleMiner;
    }

    public void PickFertileSoil()
    {
        BuildSystem._Instance.ChosseObject(B_fertileSoil);
        currentSelected = fertileSoil;
    }
}