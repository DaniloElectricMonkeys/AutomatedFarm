using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AutomatedFarm
{
    public class BlueprintObstructionCheck : MonoBehaviour
    {
        public LayerMask blockedLayers;
        public LayerMask allowedLayers;
        Vector3 rayStart;

        private void Update() {

            rayStart = NewGrid.Instance.GetGridPoint(transform.position + new Vector3(0,10,0));

            if(Physics.Raycast(rayStart, -transform.up, 100f, allowedLayers))//Hit what i wanted
            {
                if(Physics.Raycast(rayStart, -transform.up, 100f, blockedLayers))//Check if i also hit some that i dont wanted
                {
                    BuildSystem.Instance.obstructed = true;
                    return;
                }

                BuildSystem.Instance.obstructed = false;
                return;
            }
            else
            {
                BuildSystem.Instance.obstructed = true;
                return;
            }
        }

        private void OnDestroy() {
            BuildSystem.Instance.obstructed = false;
        }
    }
}
