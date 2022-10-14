using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_Buildings", menuName = "AutomatedFarm/SO_Buildings", order = 0)]
public class SO_Buildings : ScriptableObject {
    public List<Building> buildings = new List<Building>();
}

[System.Serializable]
public class Building
{
    public string name;
    public float price;
    public Sprite icon;
    public GameObject blueprint;
    public GameObject original;
}
