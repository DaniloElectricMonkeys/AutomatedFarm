using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

///<summary>
/// Grow plants. Simple
///</summary>
public class PlantGrow : MonoBehaviour
{
    [Header("Options")]
    string name;
    float time;
    public SeedTypes type;
    public bool canBeHarvested;
    public static Action<GameObject> OnPlantReady;
    public static Action OnPlantPlaced;
    [HideInInspector] public bool isAssignedToGraber;

    private void Start() 
    {
        OnPlantPlaced?.Invoke();
        gameObject.transform.localScale = new Vector3(0.001f,0.001f,0.001f);
        transform.DOScale(Vector3.one, time).SetEase(Ease.Linear).OnComplete((CompletePlant));
        //LeanTween.scale(gameObject, desiredScale, time);
    }

    void CompletePlant()
    {
        canBeHarvested = true;
        OnPlantReady?.Invoke(gameObject);
    }

    public void AssignToGraber() => isAssignedToGraber = true;

    public bool Harvest(){
        
        if(canBeHarvested)
        {
            transform.localScale = Vector3.zero;
            canBeHarvested = false;
            return true;
        }
        return false;
    }

    void LoadSettings(Seed settings)
    {
        name = settings.name;
        time = settings.timeToGrow;
    }
}
