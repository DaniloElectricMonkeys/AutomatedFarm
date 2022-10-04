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
    public float time;
    public Vector3 desiredScale;
    public bool canBerHarvested;
    public static Action<GameObject> OnPlantReady;
    public static Action OnPlantPlaced;
    [HideInInspector] public bool isAssignedToGraber;

    private void Start() 
    {
        OnPlantPlaced?.Invoke();
        gameObject.transform.localScale = new Vector3(0.001f,0.001f,0.001f);
        transform.DOScale(desiredScale, time).SetEase(Ease.Linear).OnComplete((CompletePlant));
        //LeanTween.scale(gameObject, desiredScale, time);
    }

    void CompletePlant()
    {
        canBerHarvested = true;
        OnPlantReady?.Invoke(gameObject);
    }

    public void AssignToGraber() => isAssignedToGraber = true;

    public void Harvest(){
        
        if(canBerHarvested)
            transform.localScale = Vector3.zero;
    }

}
