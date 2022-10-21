using System;
using System.Collections;
using System.Collections.Generic;
using MyEnums;
using UnityEngine;

public class CrystilizeMachine : OutputMachine
{
    public override void OnResourceEnter(ResourceType type, GameObject obj, int amount)
    {
        base.OnResourceEnter(type, obj, 1);
    }
}
