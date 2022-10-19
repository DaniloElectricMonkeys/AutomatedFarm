using System.Security.AccessControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyEnums;

public class ItemLibrary : Singleton<ItemLibrary>
{
    public GameObject rawCorn;
    public GameObject boiledCorn;
    public GameObject smashedCorn;
    public GameObject cookedCorn;
    public GameObject crystalCorn;
    public GameObject packedCorn;
    public GameObject cardboard;
    public GameObject sugar;
    
    public GameObject GetPrefabFromType(ResourceType type) {
        switch(type) {
            case ResourceType.corn:
                return rawCorn;
            case ResourceType.boiledCorn:
                return boiledCorn;
            case ResourceType.smashedCorn:
                return smashedCorn;
            case ResourceType.cookedCorn:
                return cookedCorn;
            case ResourceType.crystalCorn:
                return crystalCorn;
            case ResourceType.packedCorn:
                return packedCorn;
            case ResourceType.cardboard:
                return cardboard;
            case ResourceType.sugar:
                return sugar;
        }
        return null;
    }

}
