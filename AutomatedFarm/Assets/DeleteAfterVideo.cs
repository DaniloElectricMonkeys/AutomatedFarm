using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteAfterVideo : MonoBehaviour
{
    public GameObject cardboadPrefab;
    public Transform output;
    public float timer;
    float t;
    
    void Start()
    {
        // InvokeRepeating("CreatCardBoard", 2,2);
        t = timer;
    }

    private void Update() {
        if(t <= 0) {
            CreatCardBoard();
            t = timer;
        }

        t -= Time.deltaTime;

    }

    void CreatCardBoard() {
        Instantiate(cardboadPrefab, output.position, Quaternion.identity);
    }
    
}
