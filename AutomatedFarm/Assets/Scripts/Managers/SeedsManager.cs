using TMPro;
using UnityEngine;
using UnityEngine.UI;
using MyEnums;

public class SeedsManager : Singleton<SeedsManager>
{
    [SerializeField] SeedSO[] seedScriptable;
    [SerializeField] SeedContainer seedContainer;
    [SerializeField] GridLayoutGroup grid;

    [Header("Seed Details")]
    [SerializeField] TextMeshProUGUI myName;
    [SerializeField] TextMeshProUGUI price;
    [SerializeField] TextMeshProUGUI gold;
    [SerializeField] TextMeshProUGUI exp;
    [SerializeField] TextMeshProUGUI obtain;

    public Seed currentSeed { get; private set; }

    private void Start()
    {
        SpawnSeedContainer();
    }

    void SpawnSeedContainer()
    {
        for (int i = 0; i < seedScriptable[0].seeds.Length; i++)
        {
            Seed currentSeed = seedScriptable[0].seeds[i];
            SeedContainer container = Instantiate(seedContainer, grid.transform);
            container.GetSeed(currentSeed);
        }
    }

    public void UpdateSeedDetails(string seedName, float seedPrice, float seedGold, float seedExp, float seedObtained)
    {
        myName.text = seedName;
        price.text = "Price: " + seedPrice.ToString();
        gold.text = "Gold: " + seedGold.ToString();
        exp.text = "Exp: " + seedExp.ToString();
        obtain.text = "Obtain: " + seedObtained.ToString();
    }

    public void CurrentSeedSelected(Seed seed)
    {
        currentSeed = seed;
    }

    public void SeedSelected()
    {
        CanvasManager.Instance.ToggleSeedsUI();
        Library.Instance.SetCurrentSelected(currentSeed.seed);
        BuildSystem.Instance.ChosseObject(currentSeed.seedBlueprint);
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
