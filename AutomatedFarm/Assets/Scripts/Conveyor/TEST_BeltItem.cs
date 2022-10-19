using System;
using MyEnums;
using UnityEngine;

public class TEST_BeltItem : MonoBehaviour
{
    public GameObject item;
    public TEST_Belt currentConveyor;
    public ResourceType type;

    private void Awake()
    {
        item = gameObject;
    }
}