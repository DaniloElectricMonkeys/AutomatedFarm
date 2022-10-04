using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    Library lib;

    public string title;
    [TextArea(2,3)]
    public string description;

    private void Start() {
        lib = Library.Instance;
    }

    private void OnMouseEnter() => EnableToolTip();
    private void OnMouseExit() => DisableToolTip();
    public void OnPointerEnter(PointerEventData eventData) => EnableToolTip();
    public void OnPointerExit(PointerEventData eventData) => DisableToolTip();

    private void EnableToolTip()
    {
        lib.toolTip.gameObject.SetActive(true);
        lib.toolTip.title.text = title;
        lib.toolTip.description.text = description;
    }
    private void DisableToolTip()
    {
        lib.toolTip.gameObject.SetActive(false);
    }

    
}
