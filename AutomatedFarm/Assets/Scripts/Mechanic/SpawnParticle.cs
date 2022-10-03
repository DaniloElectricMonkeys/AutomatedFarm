using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnParticle : MonoBehaviour
{   
    public GameObject particle;

    public void Spawn(Vector3 pos)
    {
        Instantiate(particle, pos, Quaternion.identity);
    }
}
