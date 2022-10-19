using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using MyEnums;

public class TEST_FollowCamera : MonoBehaviour
{
    public GameObject target;
    bool doOnce;
    bool enableMove;
    public bool excludeCorn;
    public bool excludeBoiledCorn;
    public bool excludeSmashedCorn;
    public bool excludeCrystalCorn;
    public bool excludePackedCorn;
    public bool excludeCookedCorn;
    Vector3 refPos;
    // Start is called before the first frame update
    void Start()
    {
        // Time.timeScale = 2.75f;
    }

    // Update is called once per frame
    void Update()
    {
        if(target == null || target.activeSelf == false)
        {
            ConveyorItem[] itens = null;
            target = null;
            enableMove = false;
            doOnce = false;
            itens = FindObjectsOfType<ConveyorItem>();
            foreach (var item in itens)
            {
                if(target != null) return;

                if(item.type != ResourceType.cardboard && item.gameObject.activeSelf == true
                && item.type != ResourceType.sugar) 
                {
                    if(excludeCorn)
                    {
                        if(excludeBoiledCorn) 
                        {
                            if(excludeSmashedCorn)
                            {
                                if(excludeCookedCorn)
                                {
                                    if(excludeCrystalCorn)
                                    {
                                        if(excludePackedCorn)
                                            return;

                                        if(item.type != ResourceType.corn && item.type != ResourceType.boiledCorn && item.type != ResourceType.smashedCorn && item.type != ResourceType.cookedCorn && item.type != ResourceType.crystalCorn)
                                            target = item.gameObject;
                                    }

                                    if(item.type != ResourceType.corn && item.type != ResourceType.boiledCorn && item.type != ResourceType.smashedCorn && item.type != ResourceType.cookedCorn)
                                        target = item.gameObject;
                                }

                                if(item.type != ResourceType.corn && item.type != ResourceType.boiledCorn && item.type != ResourceType.smashedCorn)
                                    target = item.gameObject;
                            }

                            if(item.type != ResourceType.corn && item.type != ResourceType.boiledCorn)
                                target = item.gameObject;
                        }

                        if(item.type != ResourceType.corn)
                            target = item.gameObject;
                    }

                    target = item.gameObject;
                    if(item.type == ResourceType.corn) excludeCorn = true;
                    if(item.type == ResourceType.boiledCorn) excludeBoiledCorn = true;
                    if(item.type == ResourceType.smashedCorn) excludeSmashedCorn = true;
                    if(item.type == ResourceType.crystalCorn) excludeCrystalCorn = true;
                    if(item.type == ResourceType.packedCorn) excludePackedCorn = true;
                    if(item.type == ResourceType.cookedCorn) excludeCookedCorn = true;
                }
            }
            
        }
        if(target != null) 
        {
            refPos = Vector3.Lerp(transform.position, new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z), 0.05f);
            transform.position =  refPos;
        }
    }
}
