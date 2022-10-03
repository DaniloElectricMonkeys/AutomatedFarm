using System.Security.AccessControl;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyEnums;

public class ConveyorItem : MonoBehaviour
{
    [Tooltip("Linked object state")]
    public bool isLinked;
    [Tooltip("Enable item to be linked with the cnveyor that is moving it")]
    public bool enableLinking = true;
    Conveyor conveyorRef;
    public ResourceType type;

    public void RemoveLink(bool disableLinking = false)
    {
        if(disableLinking == true) enableLinking = false;
        isLinked = false;
        conveyorRef.RemoveLink(gameObject);
    }

    public void LinLink(Conveyor conveyor)
    {
        if(enableLinking == false) return;
        conveyorRef = conveyor;
        isLinked = true;
    }
}
