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
    GameObject createdObject;


    ///<summary>
    /// Select the desired object on the Build System and create the blueprint of it.
    ///</summary>
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
        MoveBlueprintObject(); // Move and snap the bluprint object
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
            BuildOnlyInTag tagChecker = blueprintObj.GetComponent<BuildOnlyInTag>();
            BuildPrice price = blueprintObj.GetComponent<BuildPrice>();

            // Return if player cant buy.
            if(price != null)
                if(price.CanPay() == false) return;

            if(tagChecker != null && tagChecker.tag != "")// If tag checker exist, only allow building in the selected tag and layer.
            {
                if(tagChecker.RayHitNode())// Do the cheking on tag and layer
                {
                    id++;
                    createdObject = Instantiate(Library.Instance.currentSelected, NewGrid.Instance.GetGridPoint(hit.point), blueprintObj.transform.rotation);
                    createdObject.name = "Object_" + id;
                    createdObject.GetComponent<IGrowBuild>()?.StartGrow();
                    createdObject.GetComponent<Extractor>()?.ChangeResourceType(tagChecker.GetResourceBelow());
                }
            }
            else
            {
                id++;
                createdObject = Instantiate(Library.Instance.currentSelected, NewGrid.Instance.GetGridPoint(hit.point), blueprintObj.transform.rotation);
                createdObject.name = "Object_" + id;
                createdObject.GetComponent<IGrowBuild>()?.StartGrow();
            }
        }
    }

    ///<summary>
    /// Remove the blueprint selection from the mouse.
    ///</summary>
    void RemoveSelection()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Destroy(blueprintObj);
            blueprintObj = null;
        }
    }

}
