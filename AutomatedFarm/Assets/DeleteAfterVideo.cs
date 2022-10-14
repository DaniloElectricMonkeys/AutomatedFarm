using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteAfterVideo : MonoBehaviour
{
    public GameObject cardboadPrefab;
    public Transform output;
    
    void Start()
    {
        InvokeRepeating("CreatCardBoard", 2,2);
    }

    void CreatCardBoard() {
        Instantiate(cardboadPrefab, output.position, Quaternion.identity);
    }
    
}
