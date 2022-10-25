using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_PalletItemHolder : MonoBehaviour
{
    public List<GameObject> itens = new List<GameObject>();
    public Transform parent;

    private void Start() 
    {
        for (int i = 0; i < parent.childCount; i++)
            itens.Add(parent.GetChild(i).gameObject);

        foreach (var item in itens)
        {
            item.SetActive(false);
        }
    }

    public void StackPallet()
    {
        foreach (var item in itens)
        {
            if(item.activeInHierarchy == false)
            {
                item.SetActive(true);
                return;
            }
        }
    }

}
