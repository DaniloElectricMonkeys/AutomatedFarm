using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.UI;
using TMPro;
using UnityEngine.UI;

public class SeedsManager : Singleton<SeedsManager>
{
    [SerializeField] SeedSO[] seedScriptable;
    [SerializeField] SeedContainer seedContainer;
    [SerializeField] GridLayoutGroup grid;

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
            container.Settings(currentSeed.icon, currentSeed.timeToGrow);
        }
    }
}
