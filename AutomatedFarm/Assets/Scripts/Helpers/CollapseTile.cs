using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollapseTile : MonoBehaviour
{
    public enum TileType{none, sand, water, grass, forest, deepWater, hill, morro, montanha}
    public TileType type;
    public List<bool> outcomes = new List<bool>();

    // Water, sand, gras, forest
    public List<GameObject> outcomesObjets = new List<GameObject>();
    public Transform top;
    public Transform bottom;
    public Transform left;
    public Transform right;
    public bool colapsed;

    public int GetTrueCounts()
    {
        int temp = 0;
        foreach (var item in outcomes)
        {
            if(item == true)
                temp++;
        }
        return temp;
    }

    public void CollapseCurrrentTile()
    {
        if(colapsed) return;

        List<GameObject> possibleOutcomes = new List<GameObject>();
        for (int i = 0; i < outcomes.Count; i++)
        {
            if(outcomes[i] == true)
                possibleOutcomes.Add(outcomesObjets[i]);
        }

        var id = UnityEngine.Random.Range(0, possibleOutcomes.Count);
        if(possibleOutcomes.Count == 0) return;
        possibleOutcomes[id].SetActive(true);

        if(possibleOutcomes[id].name == "Water") type = TileType.water;
        if(possibleOutcomes[id].name == "Sand") type = TileType.sand;
        if(possibleOutcomes[id].name == "Grass") type = TileType.grass;
        if(possibleOutcomes[id].name == "Forest") type = TileType.forest;
        if(possibleOutcomes[id].name == "DeepWater") type = TileType.deepWater;
        if(possibleOutcomes[id].name == "Hill") type = TileType.hill;
        if(possibleOutcomes[id].name == "Morro") type = TileType.morro;
        if(possibleOutcomes[id].name == "Montanha") type = TileType.montanha;

        colapsed = true;

        CheckSides(top, type);
        CheckSides(bottom, type);
        CheckSides(left, type);
        CheckSides(right, type);
    }

    void CheckSides(Transform pos, TileType type)
    {
        RaycastHit hit;
        Ray ray = new Ray(pos.position, Vector3.down);
        if(Physics.Raycast(ray, out hit, 15f))
        {
            hit.collider.gameObject.GetComponent<CollapseTile>()?.SetRules(type);
        }
    }

    public void SetRules(TileType type)
    {
        switch (type)
        {
            case TileType.sand:
                outcomes[0] = false;
                outcomes[4] = false;
                outcomes[5] = false;
                outcomes[6] = false;
                outcomes[7] = false;
                break;
            case TileType.forest:
                outcomes[0] = false;
                outcomes[1] = false;
                outcomes[2] = false;
                outcomes[6] = false;
                outcomes[7] = false;
                
                break;
            case TileType.water:
                outcomes[3] = false;
                outcomes[4] = false;
                outcomes[5] = false;
                outcomes[6] = false;
                outcomes[7] = false;
                break;
            case TileType.grass:
                outcomes[0] = false;
                outcomes[1] = false;
                outcomes[5] = false;
                outcomes[6] = false;
                outcomes[7] = false;
                break;
            case TileType.deepWater:
                outcomes[2] = false;
                outcomes[3] = false;
                outcomes[4] = false;
                outcomes[5] = false;
                outcomes[6] = false;
                outcomes[7] = false;
                break;
            case TileType.hill:
                outcomes[0] = false;
                outcomes[1] = false;
                outcomes[2] = false;
                outcomes[3] = false;
                // outcomes[4] = false;
                // outcomes[5] = false;
                // outcomes[6] = false;
                outcomes[7] = false;
                break;
            case TileType.morro:
                outcomes[0] = false;
                outcomes[1] = false;
                outcomes[2] = false;
                outcomes[3] = false;
                outcomes[4] = false;
                // outcomes[5] = false;
                // outcomes[6] = false;
                // outcomes[7] = false;
                break;
            case TileType.montanha:
                outcomes[0] = false;
                outcomes[1] = false;
                outcomes[2] = false;
                outcomes[3] = false;
                outcomes[4] = false;
                outcomes[5] = false;
                // outcomes[6] = false;
                // outcomes[7] = false;
                break;
            
        }
    }
}

[Serializable]
public class Rules
{
    public GameObject tile;
    public List<GameObject> acceptTiles = new List<GameObject>();
}
