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
        // for (int i = 0; i < itensInConveyor.Count; i++)
        // {
        //     // if(itensInConveyor[i].GetComponent<ConveyorItem>().UsedByOther(GetComponent<Collider>()) == false)
        //         itensInConveyor[i].transform.position += (end.position - itensInConveyor[i].transform.position).normalized * speed * Time.deltaTime;
        // }

        foreach (ConveyorItem item in itensInConveyor)
        {
            if(item == null) return;

            item.transform.position += (end.position - item.transform.position).normalized * speed * Time.deltaTime;
            if(GetToleranceDistance(item.transform.position, end.position, 0.2f))
                removeItens.Add(item);
            // if(item.transform.position.y < transform.position.y -0.2f)
            //     removeItens.Add(item);
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
        Debug.Log("Linking");
    }

    public void RemoveLinkItem(ConveyorItem item) {
        if(itensInConveyor.Contains(item))
            itensInConveyor.Remove(item);
        
        removeItens.Remove(item);
        item.conveyorRef = null;
        item.isLinked = false;

        item.CheckForNewConveyor();
        Debug.Log("Removeing");
    }
    
    private void OnDestroy() {
        OnConveyorDeleted?.Invoke();
    }

}
