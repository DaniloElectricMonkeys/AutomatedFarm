using UnityEngine;

public class CanvasManager : Singleton<CanvasManager>
{
    public RectTransform seedsUI;
    public RectTransform buttonsUI;
    public RectTransform cameraSwitch;

    public void ToggleSeedsUI()
    {
        seedsUI.gameObject.SetActive(!seedsUI.gameObject.activeInHierarchy);

        if (seedsUI.gameObject.activeInHierarchy)
        {

        }
        buttonsUI.gameObject.SetActive(!buttonsUI.gameObject.activeInHierarchy);
        cameraSwitch.gameObject.SetActive(!cameraSwitch.gameObject.activeInHierarchy);
    }

    public void ToggleCamera()
    {
        CameraManager.Instance.ToggleCamera();
    }
}
