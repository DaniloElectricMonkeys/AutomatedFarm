using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyEnums;
using System;

public class BoilerMachine : OutputMachine
{
    public override void OnResourceEnter(ResourceType type, GameObject obj, int amout = 0)
    {
        base.OnResourceEnter(type, obj, 1);
    }
}
