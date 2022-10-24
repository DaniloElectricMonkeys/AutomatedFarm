using System;
using MyEnums;
using UnityEngine;

public class TEST_BeltItem : MonoBehaviour
{
    public GameObject item;
    public TEST_Belt currentConveyor;
    public ResourceType type;

    Ray ray;
    RaycastHit hit;
    public LayerMask conveyorTurnLayer;

    Vector3 currentUp;
    public bool onRamp;
    private void Awake()
    {
        item = gameObject;
        currentUp = Vector3.up;
    }

    private void Update() 
    {
        if(currentConveyor != null && currentConveyor.rampNormal != null)
        {
            currentUp = Vector3.Lerp(currentUp, currentConveyor.rampNormal.transform.up, 0.10f);
            item.transform.up = currentUp;
            onRamp = true;
        }
        else if(!onRamp)
        {
            onRamp = false;
            ray = new Ray(transform.position, Vector3.down);
            if(Physics.Raycast(ray, out hit, 0.5f, conveyorTurnLayer))
            {
                currentUp = Vector3.Lerp(currentUp, hit.normal, 0.15f);
                item.transform.up = currentUp;
            }
        }
        else
            onRamp = false;
    }
}