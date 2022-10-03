using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

///<summary>
/// Grow plants. Simple
///</summary>
public class PlantGrow : MonoBehaviour
{
    [Header("Options")]
    public float time;
    public Vector3 desiredScale;

    private void Start() 
    {
        gameObject.transform.localScale = new Vector3(0.001f,0.001f,0.001f);
        transform.DOScale(desiredScale, time).SetEase(Ease.Linear);
        //LeanTween.scale(gameObject, desiredScale, time);
    }

}
