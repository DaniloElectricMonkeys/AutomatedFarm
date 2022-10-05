using UnityEngine;
using UnityEngine.UI;

public enum SeedTypes { Corn, Apple, Pumpkin }

[CreateAssetMenu(fileName = "WorldSeed", menuName = "Harvestable Seeds")]
public class SeedSO : ScriptableObject
{
    public Seed[] seeds;

    public Seed GetSeed(SeedTypes type)
    {
        for (int i = 0; i < seeds.Length; i++)
        {
            if (type == seeds[i].type) return seeds[i];
        }

        return null;
    }
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
    public SeedTypes type;
    public float timeToGrow;
}
