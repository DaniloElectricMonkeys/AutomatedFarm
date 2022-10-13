using System.Net;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
/// Handle converyor logic. Moving objects.
///</summary>
[RequireComponent(typeof(Rigidbody))]
public class Conveyor : MonoBehaviour
{
    public static Action OnConveyorDeleted;
    Rigidbody rb;
    public Transform end;
    List<ConveyorItem> itensInConveyor = new List<ConveyorItem>();
    List<ConveyorItem> removeItens = new List<ConveyorItem>();

    [Header("Speed")]
    public float speed;
    ConveyorItem item;
    bool inTrigger;

    private void Start() => rb = GetComponent<Rigidbody>();

    private void OnTriggerEnter(Collider other) 
    {
        item = other.gameObject.GetComponent<ConveyorItem>();

        if(item != null && item.isLinked == false) {
            LinkItem(item);
        }
    }

    private void Update() 
    {
        removeItens.Clear();

        foreach (ConveyorItem item in itensInConveyor)
        {
            if(item == null) return;

            item.transform.position += (end.position - item.transform.position).normalized * speed * Time.deltaTime;
            if(GetToleranceDistance(item.transform.position, end.position, 0.2f))
                removeItens.Add(item);
            if(!item.gameObject.activeSelf)
                removeItens.Add(item);
            if(item.conveyorRef != this)
                removeItens.Add(item);
        }

        // Remove link between item and conveyor. Release item to be linked by other conveyors or machines.
        foreach (ConveyorItem item in removeItens.ToArray())
        {
            if(item != null)
                RemoveLinkItem(item);
        }
    }
    
    bool GetToleranceDistance(Vector3 start, Vector3 end, float tolerance)
    {
        return ((end - start).magnitude <= tolerance);
    }

    public void LinkItem(ConveyorItem item) {
        itensInConveyor.Add(item);
        item.conveyorRef = this;
        item.isLinked = true;
    }

    public void RemoveLinkItem(ConveyorItem item) {
        if(itensInConveyor.Contains(item))
            itensInConveyor.Remove(item);
        
        removeItens.Remove(item);
        item.conveyorRef = null;
        item.isLinked = false;

        item.CheckForNewConveyor();
    }
    
    private void OnDestroy() {
        OnConveyorDeleted?.Invoke();
    }

}
