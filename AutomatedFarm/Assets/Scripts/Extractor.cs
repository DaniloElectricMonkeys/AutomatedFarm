using UnityEngine;
using MyEnums;

[RequireComponent(typeof(SingleMiner))]
public class Extractor : MonoBehaviour
{
    // This class is responsable for creating the resources from the resouce nodes.

    SingleMiner miner;
    [Header("Options")]
    public ResourceType resourceType;
    public Transform spawnPoint;
    float time;
    GameObject go;

    private void Start() 
    {
        miner = GetComponent<SingleMiner>();
        miner.OnSpawn += CreateResource;
    }

    void CreateResource()
    {
        switch (resourceType)
        {
            case ResourceType.soil:
                go = ObjectPool.Instance.GrabFromPool("Soil", Library.Instance.soilPrefab);
                break;
            case ResourceType.ore:
                go = ObjectPool.Instance.GrabFromPool("Ore", Library.Instance.orePrefab);
                break;
            case ResourceType.stone:
                go = ObjectPool.Instance.GrabFromPool("Stone", Library.Instance.stonePrefab);
                break;
        }
        
        go.transform.position = spawnPoint.position;
        go.transform.rotation = Quaternion.identity;
        go.SetActive(true);
    }

    public void ChangeResourceType(ResourceType type)
    {
        resourceType = type;
    }

    private void OnDestroy() 
    {
        miner.OnSpawn -= CreateResource;
    }

    private void OnDisable() 
    {
        miner.OnSpawn -= CreateResource;
    }

}
