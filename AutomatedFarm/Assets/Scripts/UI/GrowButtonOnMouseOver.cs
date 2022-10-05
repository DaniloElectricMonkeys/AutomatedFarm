using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class GrowButtonOnMouseOver : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    Vector3 scale;
    Tween tween;

    private void Start() {
        scale = transform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Vector3 endScale = transform.localScale * 1.2f;
        tween = transform.DOScale(endScale, 0.1f).SetEase(Ease.OutCubic);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tween.Kill();
        transform.localScale = Vector3.one;
    }
}
