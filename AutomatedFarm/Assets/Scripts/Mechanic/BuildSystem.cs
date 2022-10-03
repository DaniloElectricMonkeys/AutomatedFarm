using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BuildSystem : MonoBehaviour
{
    public static BuildSystem _Instance;
    private void Awake() => _Instance = this;
    public GameObject blueprintObj;
    int rotation;
    int rotationIndex;
    int id = 0;
    RaycastHit hit {get { return MouseRaycast.Instance.FireRaycast(); }}

    public void ChosseObject(GameObject obj)
    {
        if(obj != null && blueprintObj == null)
        {
            blueprintObj = Instantiate(obj, hit.point, Quaternion.identity);
            blueprintObj.tag = "Untagged";
        }
            
    }

    private void Update() 
    {
        MoveBlueprintObject();
        BuildBlueprintObj();
        RemoveSelection();
        RotateSelection();
    }

    public void RotateSelection()
    {
        if(blueprintObj != null)
        {
            if(Input.GetKeyDown(KeyCode.R))
            {
                rotationIndex += 1;
                rotation = rotationIndex * 90;
                Vector3 newRotation = (Quaternion.Euler(0, rotation, 0)).eulerAngles;
                blueprintObj.transform.DORotate(newRotation,0.45f).SetEase(Ease.InOutElastic);
                //blueprintObj.transform.LeanRotateY(rotation, .45f).setEaseInOutElastic();

                if(rotationIndex >= 4)
                    rotationIndex = 0;
            }
        }
    }

    void MoveBlueprintObject()
    {
        if(blueprintObj != null)
        {
            if(blueprintObj.GetComponent<SnapToGrid>() == null)
                blueprintObj.AddComponent<SnapToGrid>();

            blueprintObj.transform.position = hit.point;
        }
    }

    void BuildBlueprintObj()
    {
        if(blueprintObj != null && Input.GetMouseButtonDown(0))
        {
            BuildOnlyInTag boit = blueprintObj.GetComponent<BuildOnlyInTag>();
            BuildPrice bp = blueprintObj.GetComponent<BuildPrice>();

            if(bp != null)
                if(bp.CanPay() == false) return;

            if(boit != null && boit.tag != "")
            {
                if(boit.CastCheckRay())
                {
                    id++;
                    GameObject go = Instantiate(Library.Instance.currentSelected, NewGrid.instance.GetGridPoint(hit.point), blueprintObj.transform.rotation);
                    go.name = "Object_" + id;
                    go.GetComponent<IGrowBuild>()?.StartGrow();
                    go.GetComponent<Extractor>()?.ChangeResourceType(boit.GetResourceBelow());
                }
            }
            else
            {
                id++;
                GameObject go = Instantiate(Library.Instance.currentSelected, NewGrid.instance.GetGridPoint(hit.point), blueprintObj.transform.rotation);
                go.name = "Object_" + id;
                go.GetComponent<IGrowBuild>()?.StartGrow();
            }
        }
    }

    void RemoveSelection()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Destroy(blueprintObj);
            blueprintObj = null;
        }
    }

}
