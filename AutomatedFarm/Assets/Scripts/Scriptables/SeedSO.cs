using UnityEngine;

[CreateAssetMenu(fileName = "WorldSeed", menuName = "Harvestable Seeds")]
public class SeedSO : ScriptableObject
{
    public Seed[] seeds;
}

[System.Serializable]
public class Seed
{
    public string name;
    public Sprite icon;
    public float price;
    public float goldObtained;
    public float expObtained;
    public float amountObtained;
    public GameObject seed;
    public GameObject seedBlueprint;
    public float timeToGrow;
}
