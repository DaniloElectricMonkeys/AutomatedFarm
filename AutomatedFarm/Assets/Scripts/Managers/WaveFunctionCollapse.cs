using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WaveFunctionCollapse : MonoBehaviour
{
    public GameObject tileCollapse;
    public Transform tileParent;
    public int sizeX, sizeY;
    public List<GameObject> tilesToCollapse = new List<GameObject>();

    public void GenerateWorld()
    {
        Build();
    }

    void Build()
    {
        if (tilesToCollapse.Count > 0)
        {
            for (int i = tilesToCollapse.Count - 1; i >= 0; i--)
            {
                DestroyImmediate(tilesToCollapse[i]);
            }
            tilesToCollapse.Clear();
        }

        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeY; j++)
            {
                Vector3 spawnPosition = new Vector3(i * 3, 0, j * 3);
                tilesToCollapse.Add(Instantiate(tileCollapse, spawnPosition, Quaternion.identity, tileParent));
            }
        }

        List<CollapseTile> notColapsed = new List<CollapseTile>();
        foreach (var item in tilesToCollapse)
        {
            if (item.GetComponent<CollapseTile>().colapsed == false)
            {
                notColapsed.Add(item.GetComponent<CollapseTile>());
            }
        }

        while (notColapsed.Count > 0)
        {
            int temp = 10;
            CollapseTile tile = null;
            foreach (var item in notColapsed)
            {
                if (item.GetTrueCounts() < temp)
                {
                    temp = item.GetTrueCounts();
                    tile = item;
                }
            }
            tile.CollapseCurrrentTile();
            notColapsed.Remove(tile);
        }
    }
}


#if UNITY_EDITOR
    [CustomEditor(typeof(WaveFunctionCollapse))]
    public class WaveFunctionCollapseEditor : Editor {
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
    
            if(GUILayout.Button("Generate World"))
            {
                WaveFunctionCollapse waveFunction = (WaveFunctionCollapse)target;
                waveFunction.GenerateWorld();
            }
        }
    }
#endif
