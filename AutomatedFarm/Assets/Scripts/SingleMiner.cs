using System.Diagnostics.Contracts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleMiner : MonoBehaviour
{
    [Header("SpawOptions")]
    public float curveSpawnTime;
    [Header("AnimOptions")]
    public AnimationCurve animCurve;
    public float animSpeed;
    float curveTime;
    float value;
    public GameObject[] animRoot;
    bool spawnObj;
    public Action OnSpawn;
    [Header("OutputOptions")]
    public bool isConnected;
    public GameObject outputPoint;


    // Start is called before the first frame update
    public float GetTime()
    {
        return curveTime;
    }

    public virtual void Start()
    {
        spawnObj = true;
        Debug.Log("Start");
        StartCoroutine(DoAnim());
    }

    IEnumerator DoAnim()
    {
        Debug.Log("Anim");
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
                {
                    animRoot[i].transform.localPosition = new Vector3(0,value,0);    
                }

                // Spawn
                if(curveTime >= curveSpawnTime && spawnObj == true)
                {
                    OnSpawn?.Invoke();
                    spawnObj = false;
                    CheckOutput();
                }

                // Reset curve
                if(curveTime >= 1)
                {
                    //CheckOutput();
                    curveTime = 0;
                    spawnObj = true;
                }
            }
            else
            {
                CheckOutput();
                Debug.Log("Checkd");
            }
        }
    }

    ///<summary>
    /// Check if there is a conveyor below the output
    ///</summary>
    void CheckOutput()
    {
        if(Physics.Raycast(outputPoint.transform.position, Vector3.down, out RaycastHit hit, 10f))
        {
            if(hit.collider.gameObject.CompareTag("Conveyor") && isConnected == false)
            {
                isConnected = true;
                SideCheck[] checkers = hit.collider.gameObject.GetComponentsInChildren<SideCheck>();
                if(checkers == null) return;

                foreach (SideCheck item in checkers)
                {
                    item.CheckForMachineConnection();
                }
            }
            else
                isConnected = false;
        }
    }
}
