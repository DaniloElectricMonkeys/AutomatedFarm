using UnityEngine;

public class CanvasManager : Singleton<CanvasManager>
{
    public RectTransform seedsUI;
    public RectTransform machinesUI;
    public RectTransform buttonsUI;
    public RectTransform cameraSwitch;
    [SerializeField] CanvasGroup bottomUI;

    public void ToggleSeedsUI()
    {
        seedsUI.gameObject.SetActive(!seedsUI.gameObject.activeInHierarchy);

        if (seedsUI.gameObject.activeInHierarchy && SeedsManager.Instance.firstContainer != null)
        {
            SeedsManager.Instance.SelectFirst();
        }
        buttonsUI.gameObject.SetActive(!buttonsUI.gameObject.activeInHierarchy);
        cameraSwitch.gameObject.SetActive(!cameraSwitch.gameObject.activeInHierarchy);
    }

    public void ToggleMachinesUI()
    {
        machinesUI.gameObject.SetActive(!machinesUI.gameObject.activeInHierarchy);

        /*if (machinesUI.gameObject.activeInHierarchy && MachinesManager.Instance.firstContainer != null)
        {
            MachinesManager.Instance.SelectFirst();
        }*/

        buttonsUI.gameObject.SetActive(!buttonsUI.gameObject.activeInHierarchy);
        cameraSwitch.gameObject.SetActive(!cameraSwitch.gameObject.activeInHierarchy);
    }

    public void ToggleCamera()
    {
        CameraManager.Instance.ToggleCamera();
    }

    public void SelectBuilding(int ID)
    {
        Library.Instance.ChooseBuilding(ID);
    }
}
