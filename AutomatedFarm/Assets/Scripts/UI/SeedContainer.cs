using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SeedContainer : MonoBehaviour
{
    [SerializeField] Image icon;
    [SerializeField] TextMeshProUGUI growTime;
    public Seed seed { get; private set; }

    private void Start()
    {
        LoadSettings();
    }

    public void GetSeed(Seed seedSettings)
    {
        seed = seedSettings;
    }

    void LoadSettings()
    {
        icon.sprite = seed.icon;
        growTime.text = TimeSpan.FromSeconds(seed.timeToGrow).ToString(@"m\:ss");
    }

    public void Selected()
    {
        SeedsManager.Instance.UpdateSeedDetails(seed.name, seed.price, seed.goldObtained, seed.expObtained, seed.amountObtained);
        SeedsManager.Instance.CurrentSeedSelected(seed);
    }
}
