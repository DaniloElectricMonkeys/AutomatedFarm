using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class MouseTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string title;
    [TextArea(2,3)]
    public string description;

    #region For game objects
        private void OnMouseEnter() => TooTipManager.Show(title, description);
        private void OnMouseExit() => TooTipManager.Hide();
    #endregion


    #region For UI
        public void OnPointerEnter(PointerEventData eventData)
        {
            StartCoroutine(ShowTooltipDelay(0.5f));
        }
        public void OnPointerExit(PointerEventData eventData)
        {
            StopAllCoroutines();
            TooTipManager.Hide();
        }
    #endregion

    IEnumerator ShowTooltipDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        TooTipManager.Show(title, description);

    }
}
