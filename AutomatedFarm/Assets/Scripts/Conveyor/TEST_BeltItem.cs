using System;
using UnityEngine;

public class TEST_BeltItem : MonoBehaviour
{
    public GameObject item;

    private void Awake()
    {
        item = gameObject;
    }
}