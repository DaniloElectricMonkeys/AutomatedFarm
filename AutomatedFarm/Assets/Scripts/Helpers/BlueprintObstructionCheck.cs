using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueprintObstructionCheck : MonoBehaviour
{
    public LayerMask machineLayer;
    Vector3 rayStart;

    private void Update() {

        rayStart = NewGrid.Instance.GetGridPoint(transform.position + new Vector3(0,10,0));
        if(Physics.Raycast(rayStart, -transform.up, 100f, machineLayer))
            BuildSystem.Instance.obstructed = true;
        else
            BuildSystem.Instance.obstructed = false;
    }

    private void OnDestroy() {
        BuildSystem.Instance.obstructed = false;
    }
}
