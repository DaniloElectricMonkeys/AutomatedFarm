using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyEnums;
using System;

namespace AutomatedFarm
{
    public class Smasher : OutputMachine
    {
        public override void OnResourceEnter(ResourceType type, GameObject obj, int amout)
        {
            base.OnResourceEnter(type, obj, 1);
        }
    }
}
