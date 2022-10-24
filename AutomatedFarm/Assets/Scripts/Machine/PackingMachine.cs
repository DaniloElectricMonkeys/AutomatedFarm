using System;
using System.Collections;
using System.Collections.Generic;
using MyEnums;
using UnityEngine;

namespace AutomatedFarm
{
    public class PackingMachine : OutputMachine
    {
        public override void OnResourceEnter(ResourceType type, GameObject obj, int amout)
        {
            base.OnResourceEnter(type, obj, 1);
        }
    }
}
