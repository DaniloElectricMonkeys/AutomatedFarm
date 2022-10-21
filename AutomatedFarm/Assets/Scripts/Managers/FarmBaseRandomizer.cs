using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class FarmBaseRandomizer : MonoBehaviour
{
    public List<GameObject> tiles = new List<GameObject>();
    public Vector3 gridSize;
    public int xSize;
    public int zSize;
    public Transform parent;

    public void CreatTiles() {
        
        List<GameObject> remove = new List<GameObject>();
        if(parent.childCount > 0) {
            for (int i = 0; i < parent.childCount; i++)
                remove.Add(parent.GetChild(i).gameObject);
        }
        for (int i = remove.Count - 1; i >= 0 ; i--)
        {
            DestroyImmediate(parent.GetChild(i).gameObject);
        }
        
        remove.Clear();

        for (int i = 0; i < xSize; i++)
        {
            for (int h = 0; h < zSize; h++)
            {
                Instantiate(tiles[Random.Range(0, tiles.Count)], new Vector3(i*gridSize.x, 0, h*gridSize.z), Quaternion.identity, parent);
            }
        }
    }

}

// [CustomEditor(typeof(FarmBaseRandomizer))]
// public class FarmBaseRandomizerEditor : Editor {
//     public override void OnInspectorGUI() {
//         base.OnInspectorGUI();

//         FarmBaseRandomizer randomizer = (FarmBaseRandomizer)target;

//         if(GUILayout.Button("Randomize Tiles")) {
//             randomizer.CreatTiles();
//         }
//     }
// }