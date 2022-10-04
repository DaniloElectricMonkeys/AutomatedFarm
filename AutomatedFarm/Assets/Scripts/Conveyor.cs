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
    Rigidbody rb;
    public Transform start;
    public Transform end;
    public List<GameObject> itensInConveyor = new List<GameObject>();

    [Header("Speed")]
    public float speed;

    public List<GameObject> removeItens = new List<GameObject>();
    ConveyorItem item;

    private void Start() => rb = GetComponent<Rigidbody>();

    private void OnTriggerStay(Collider other) 
    {
        item = other.gameObject.GetComponent<ConveyorItem>();

        if(item != null)
        {
            //item.transform.position = Vector3.MoveTowards(item.transform.position, end.position, speed * Time.deltaTime);
            if(item.isLinked == false)
            {
                itensInConveyor.Add(other.gameObject);
                item.Link(this);
            }
        }
    }

    private void Update() 
    {
        foreach (GameObject item in itensInConveyor)
        {
            if(item == null) return;

            item.transform.position = Vector3.MoveTowards(item.transform.position, end.position, speed * Time.deltaTime);
            if(GetToleranceDistance(item.transform.position, end.position, 0.2f))
                removeItens.Add(item);
            if(item.transform.position.y <= 0.2f)
                removeItens.Add(item);
        }

        // Remove link between item and conveyor. Release item to be linked by other conveyors or machines.
        foreach (GameObject item in removeItens.ToArray())
        {
            if(item != null)
                item.GetComponent<ConveyorItem>().RemoveLink();
        }
    }
    
    bool GetToleranceDistance(Vector3 start, Vector3 end, float tolerance)
    {
        return (end - start).magnitude <= tolerance;
    }

    ///<summary>
    /// Remove item from the converyor.
    ///</summary>
    public void RemoveConveyorItem(GameObject conveyorItem)
    {
        itensInConveyor.Remove(conveyorItem);
        removeItens.Remove(conveyorItem);
    }
    
}
