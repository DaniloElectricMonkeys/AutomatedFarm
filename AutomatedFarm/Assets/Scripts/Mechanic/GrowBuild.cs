using System;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(SpawnParticle))]
public class GrowBuild : MonoBehaviour, IGrowBuild, ISpawnParticle
{
    public Vector3 startSize = new Vector3(.1f,.1f,.1f);
    public Vector3 finalSize = new Vector3(1,1,1);
    public float growTime = .45f;
    float timeRef = 0;

    public event Action OnGrowEnd;

    public void SpawnParticle()
    {
        GetComponent<SpawnParticle>().Spawn(transform.position);
    }

    public void StartGrow()
    {
        SpawnParticle();
        transform.localScale = startSize;
        transform.DOScale(finalSize, growTime).SetEase(Ease.InOutBack).OnComplete(() => OnGrowEnd?.Invoke());
        //LeanTween.scale(this.gameObject, finalSize, growTime).setEaseInOutBack().setOnComplete(() => OnGrowEnd?.Invoke());
    }
}
