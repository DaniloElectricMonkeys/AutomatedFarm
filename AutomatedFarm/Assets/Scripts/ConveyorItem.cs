using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyEnums;
using DG.Tweening;

///<summary>
/// Handle conveyour itens logic.
///</summary>
public class ConveyorItem : MonoBehaviour
{
    [Header("Layer")]
    public LayerMask machineLayer;

    public bool isLinked;
    public Conveyor conveyorRef;
    public ResourceType type;
    public bool dontKill;
    Rigidbody rb;
    Vector3 dir;

    private void Start() {
        rb = GetComponent<Rigidbody>();
    }

    public void FreshSpawnItem()
    {
        conveyorRef = null;
        dontKill = true;
        isLinked = false;
    }

    public void MoveOutFromTheMachine(Vector3 direction){
        dir = direction;
    }

    private void Update() {

        if(dontKill)
            transform.position += dir * 1 * Time.deltaTime;
    }

    private void OnTriggerExit(Collider other) {
        if(other.CompareTag("Machine"))
            dontKill = false;
    }

    public void CheckForNewConveyor()
    {
        Collider[] colls = Physics.OverlapBox(transform.position, new Vector3(0.1f,0.5f,0.1f), Quaternion.identity, machineLayer);

        foreach (var item in colls)
        {
            if(item.GetComponent<Conveyor>()) {
                item.GetComponent<Conveyor>()?.LinkItem(this);
                break;
            }

        }
    }

    private void OnDrawGizmos() {
        Gizmos.DrawWireCube(transform.position, new Vector3(0.6f,0.6f,0.6f));
    }
}
