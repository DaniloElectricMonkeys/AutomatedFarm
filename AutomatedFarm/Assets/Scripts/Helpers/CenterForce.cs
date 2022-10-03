using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterForce : MonoBehaviour
{
    public float speed;
    public List<Rigidbody> objects = new List<Rigidbody>();
    public List<Vector3> direction = new List<Vector3>();
    public Transform target;

    private void FixedUpdate() 
    {
        for (int i = 0; i < objects.Count; i++)
        {
            objects[i].transform.position += direction[i] * Time.deltaTime * speed;
        }
    }

    private void OnTriggerEnter(Collider other) 
    {
        if(other.CompareTag("Ore"))
        {
            objects.Add(other.gameObject.GetComponent<Rigidbody>());
            direction.Add((target.position - other.gameObject.transform.position).normalized);
        }
    }

    private void OnTriggerExit(Collider other) 
    {
        if(other.CompareTag("Ore"))
        {
            direction.Remove(direction[objects.IndexOf(other.gameObject.GetComponent<Rigidbody>())]);
            objects.Remove(other.gameObject.GetComponent<Rigidbody>());
        }
    }
}
