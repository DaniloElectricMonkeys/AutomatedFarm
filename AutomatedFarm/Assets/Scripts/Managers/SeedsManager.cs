using TMPro;
using UnityEngine;
using UnityEngine.UI;
using MyEnums;

namespace AutomatedFarm
{
    public class SeedsManager : Singleton<SeedsManager>
    {
        [SerializeField] SeedContainer seedContainer;
        [SerializeField] GridLayoutGroup grid;

        [Header("Seed Details")]
        [SerializeField] TextMeshProUGUI myName;
        [SerializeField] TextMeshProUGUI price;
        [SerializeField] TextMeshProUGUI gold;
        [SerializeField] TextMeshProUGUI exp;
        [SerializeField] TextMeshProUGUI obtain;

        public Seed currentSeed { get; private set; }
        public SeedContainer firstContainer { get; private set; }

        private void Start()
        {
            SpawnSeedContainer();
            SelectFirst();
        }

        void SpawnSeedContainer()
        {
            for (int i = 0; i < Library.Instance.seedScriptable[0].seeds.Length; i++)
            {
                Seed currentSeed = Library.Instance.seedScriptable[0].seeds[i];
                SeedContainer container = Instantiate(seedContainer, grid.transform);
                if (i == 0) firstContainer = container;
                container.GetSeed(currentSeed);
            }
        }

        public void UpdateSeedDetails(string seedName, float seedPrice, float seedGold, float seedExp, float seedObtained)
        {
            myName.text = seedName;
            price.text = seedPrice.ToString();
            gold.text = seedGold.ToString();
            exp.text = seedExp.ToString();
            obtain.text = seedObtained.ToString();
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
            BuildSystem.Instance.buildPrice = currentSeed.price;
        }

        public void ToggleSeedsUI()
        {
            CanvasManager.Instance.ToggleSeedsUI();
        }

        public void SelectFirst()
        {
            firstContainer.FirstSelect();
        }
    }

}
