using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

///<summary>
/// This class handle the building system.
/// Putting objects on the mouse.
/// Instantiating objects such machines and conveyors.
/// Moving them and rotating them.
///</summary>
public class BuildSystem : Singleton<BuildSystem>
{
    [Header("Attributes")]
    public GameObject blueprintObj;
    public bool obstructed;
    public LayerMask machineLayer;

    [Space]
    [Header("Materials")]
    public Material blueprintBlue;
    public Material blueprintRed;
    
    int rotation;
    int rotationIndex;
    int id = 0;
    RaycastHit hit {get { return MouseRaycast.Instance.FireRaycast(); }}
    RaycastHit machineHit {get { return MouseRaycast.Instance.FireRaycast(machineLayer); }}
    GameObject createdObject;
    bool doOnce;


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
        if(obstructed && blueprintObj != null)
        {
            foreach (MeshRenderer item in blueprintObj.GetComponentsInChildren<MeshRenderer>())
                item.material = blueprintRed;
            doOnce = false;
        }
            
        else if(!obstructed && blueprintObj != null && !doOnce)
        {
            foreach (MeshRenderer item in blueprintObj.GetComponentsInChildren<MeshRenderer>())
                item.material = blueprintBlue;
                doOnce = true;
                Debug.Log("Once");
        }


        MoveBlueprintObject(); // Move and snap the bluprint object
        if(!obstructed)
            BuildBlueprintObj();// Build the object blueprint that was on the mouse
        RemoveSelection();// Remove the object from the mouse (press ESC)
        RotateSelection();// Rotate the object, press R.
        DeleteMachine();
    }

    void DeleteMachine()
    {
        if(Input.GetKey(KeyCode.LeftControl) && Input.GetMouseButtonDown(0))
        {
            Debug.Log("Called Delete");
            if(machineHit.collider != null)
                Destroy(machineHit.collider.gameObject);
        }
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
