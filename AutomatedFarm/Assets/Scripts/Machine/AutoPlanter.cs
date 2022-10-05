using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoPlanter : Machine
{
    [Header("Attributes")]
    public float influenceArea;
    List<Collider> cachedTiles = new List<Collider>();
    List<Collider> hitTiles = new List<Collider>();
}
