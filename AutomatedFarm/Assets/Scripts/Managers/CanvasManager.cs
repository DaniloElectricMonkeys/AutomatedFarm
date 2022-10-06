using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : Singleton<CanvasManager>
{
    public RectTransform seedsUI;

    public void ToggleSeedsUI()
    {
        seedsUI.gameObject.SetActive(!seedsUI.gameObject.activeInHierarchy);
    }

    public void ToggleCamera()
    {
        CameraManager.Instance.ToggleCamera();
    }
}
