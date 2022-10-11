using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyEnums;

[CreateAssetMenu(fileName = "SO_Craftables", menuName = "AutomatedFarm/SO_Craftables", order = 0)]
public class SO_Craftables : ScriptableObject {
    public List<CraftableItem> Itens = new List<CraftableItem>();
}

[System.Serializable]
public class CraftableItem{
    public ResourceType craftableItem;
    public List<ResourceType> itensNeededToCraft = new List<ResourceType>();
}