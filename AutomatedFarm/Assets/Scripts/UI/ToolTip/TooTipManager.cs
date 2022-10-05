using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooTipManager : MonoBehaviour
{
    public static ToolTip toolTip;
    public ToolTip tipRefence;

    private void Awake() {
        toolTip = tipRefence;
    }

    public static void Show(string title, string description)
    {
        toolTip.gameObject.SetActive(true);
        toolTip.SetContent(title, description);
    }

    public static void Hide()
    {
        toolTip.gameObject.SetActive(false);
    }

}
