using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyEnums;

///<summary>
/// Handle conveyour itens logic.
///</summary>
public class ConveyorItem : MonoBehaviour
{
    [Tooltip("Linked object state")]
    public bool isLinked;
    [Tooltip("Enable item to be linked with the cnveyor that is moving it")]
    public bool enableLinking = true;
    Conveyor conveyorRef;
    public ResourceType type;
    float timer;
    Vector3 lastPos;
    public bool dontKill;

    private void Update() {

        if(transform.position == lastPos)
        {
            timer += Time.deltaTime;
            lastPos = transform.position;
            if(timer >= 1)
                RemoveLink();
        }
        else
        {
            lastPos = transform.position;
        }
        
    }

    private void OnTriggerExit(Collider other) {
        if(other.CompareTag("Machine"))
            dontKill = false;
    }
    
    ///<summary>
    /// Remove link between item and conveyor. Release item to be linked by other conveyors or machines.
    ///</summary>
    public void RemoveLink(bool disableLinking = false)
    {
        if(disableLinking == true) enableLinking = false;
        isLinked = false;
        if(conveyorRef != null)
            conveyorRef.RemoveConveyorItem(gameObject);
    }

    ///<summary>
    /// Creat a link between iten and conveyor.
    ///</summary>
    public void Link(Conveyor conveyor)
    {
        if(enableLinking == false) return;
        conveyorRef = conveyor;
        isLinked = true;
    }
}
