using System.Net;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Conveyor : MonoBehaviour
{
    Rigidbody rb;
    public Transform start;
    public Transform end;
    public List<GameObject> conveyorItens = new List<GameObject>();

    [Header("Speed")]
    public float speed;

    public List<GameObject> removeItens = new List<GameObject>();

    private void Start() => rb = GetComponent<Rigidbody>();

    private void OnTriggerStay(Collider other) 
    {
        if(other.gameObject.GetComponent<ConveyorItem>() != null)
        {
            if(other.gameObject.GetComponent<ConveyorItem>().isLinked == false)
            {
                conveyorItens.Add(other.gameObject);
                other.gameObject.GetComponent<ConveyorItem>().LinLink(this);
            }
        }
    }

    private void Update() 
    {
        foreach (GameObject item in conveyorItens)
        {
            if(item == null) return;

            item.transform.position = Vector3.MoveTowards(item.transform.position, end.position, speed * Time.deltaTime);
            if(GetToleranceDistance(item.transform.position, end.position, 0.2f))
                removeItens.Add(item);
            if(item.transform.position.y <= 0)
                removeItens.Add(item);
        }

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

    public void RemoveLink(GameObject conveyorItem)
    {
        conveyorItens.Remove(conveyorItem);
        removeItens.Remove(conveyorItem);
    }
    
}
