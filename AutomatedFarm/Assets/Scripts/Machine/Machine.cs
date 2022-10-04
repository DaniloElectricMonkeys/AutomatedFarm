using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Machine : MonoBehaviour
{
    [Header("OutputOptions")]
    public bool isConnected;
    public GameObject outputPoint;

    ///<summary>
    /// Check if there is a conveyor below the output
    ///</summary>
    protected void CheckOutput()
    {
        if(Physics.Raycast(outputPoint.transform.position, Vector3.down, out RaycastHit hit, 10f))
        {
            if(hit.collider.gameObject.CompareTag("Conveyor") && isConnected == false)
            {
                isConnected = true;
                SideCheck[] checkers = hit.collider.gameObject.GetComponentsInChildren<SideCheck>();
                if(checkers == null) return;

                foreach (SideCheck item in checkers)
                {
                    item.CheckForMachineConnection();
                }
            }
            else if(isConnected)
                return;
            else
                isConnected = false;
        }
    }
}
