using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class FeedbackTextManager : Singleton<FeedbackTextManager>
{
    [Header("Prefab")]
    public GameObject textPrefab;

    public void SpawnText(string content, Vector3 position)
    {
        GameObject textObject = Instantiate(textPrefab,  position, Quaternion.identity);
        textObject.GetComponentInChildren<TextMeshProUGUI>().text = content;
        textObject.transform.forward = Camera.main.transform.forward;
        textObject.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        textObject.transform.DOScale(new Vector3(0.5f, 0.5f, 0.5f), 1f).SetEase(Ease.OutElastic);
        textObject.GetComponentInChildren<TextMeshProUGUI>().DOFade(0, 1).SetEase(Ease.InCubic);
    }
}
