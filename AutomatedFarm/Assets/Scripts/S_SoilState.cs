using System.Collections;
using System.Collections.Generic;
using MyEnums;
using UnityEngine;

namespace AutomatedFarm
{
    public class S_SoilState : MonoBehaviour
    {
        public SoilState currentState;

        public SoilState GetState()
        {
            return currentState;
        }

        public void ChangeStateToPlanted() => currentState = SoilState.planted;
        public void ChangeStateToFree() => currentState = SoilState.free;

        public void PlantOnSoil()
        {
            GameObject go = Library.Instance.seedScriptable[0].seeds[0].seed;
            Instantiate(go, transform.position, Quaternion.identity, transform);
            ChangeStateToPlanted();
        }
    }
}
