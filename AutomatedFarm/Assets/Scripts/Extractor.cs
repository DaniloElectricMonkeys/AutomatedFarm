using System;
using UnityEngine;
using MyEnums;
using System.Collections;

///<summary>
/// This class is responsable for creating the resources from the resouce nodes.
///</summary>
public class Extractor : Machine
{
    // This class is responsable for creating the resources from the resouce nodes.

    [Header("Extraction Options")]
    public ResourceType resourceType;
    public Transform spawnPoint;
    public float timeToExtract;
    float refTimer;
    GameObject go;
    
    [Space]
    [Header("AnimOptions")]
    public AnimationCurve animCurve;
    public float animSpeed;
    float curveTime;
    float value;
    public GameObject[] animRoot;

    void Start()
    {
        StartCoroutine(DoAnim());
        refTimer = timeToExtract;
    }
    
    IEnumerator DoAnim()
    {
        yield return new WaitForSeconds(1);

        while(true)
        {
            yield return new WaitForEndOfFrame();

            if(isConnected == true)
            {
                // Animate
                curveTime += Time.deltaTime * animSpeed;
                value = animCurve.Evaluate(curveTime);

                for (int i = 0; i < animRoot.Length; i++)
                    animRoot[i].transform.localPosition = new Vector3(0,value,0);    

                if(curveTime >= 1)
                    curveTime = 0;
            }
        }
    }

    private void Update() 
    {
        refTimer -= Time.deltaTime;
        
        if(refTimer <= 0) { 
            if(!isConnected) CheckOutput();
            HarvestResource(); refTimer = timeToExtract; 
        }
    }

    void HarvestResource()
    {
        if(!isConnected) return;

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

    public void ChangeResourceType(ResourceType type) => resourceType = type;
}
