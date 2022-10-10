using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEditor.Tilemaps;
using UnityEngine.Tilemaps;

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

    [Space]
    [Header("Tilemap")]
    public TileBase tile;
    public Tilemap tilemap;

    int rotation;
    int rotationIndex;
    int id = 0;
    RaycastHit hit {get { return MouseRaycast.Instance.FireRaycast(); }}
    RaycastHit machineHit {get { return MouseRaycast.Instance.FireRaycast(machineLayer); }}
    GameObject createdObject;
    bool doOnce;
    bool canBuildAnyway;
    BuildOnlyInTag tagChecker;

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
        CheckObstructions();// Ckeck and change material in case of obstructions
        MoveBlueprintObject(); // Move and snap the bluprint object
        if(!obstructed || canBuildAnyway)//Allow to build if is not obstructed or it casn be built anyway
            BuildBlueprintObj();// Build the object blueprint that was on the mouse
        RemoveSelection();// Remove the object from the mouse (press ESC)
        RotateSelection();// Rotate the object, press R.
        DeleteMachine();
    }

    public void SetSoil()
    {
        Vector3Int tilePos = tilemap.WorldToCell(hit.point);
        tilemap.SetTile(tilePos, tile);
    }

    void CheckObstructions()
    {
        if(obstructed && blueprintObj != null)
        {
            tagChecker = blueprintObj.GetComponent<BuildOnlyInTag>();

            if(tagChecker != null && tagChecker.tag != "")
            {
                if(tagChecker.RayHitNode())
                {
                    canBuildAnyway = true;
                    foreach (MeshRenderer item in blueprintObj.GetComponentsInChildren<MeshRenderer>())
                        item.material = blueprintBlue;
                    return;
                }
                else
                    canBuildAnyway = false;
            }
            else
                canBuildAnyway = false;
                

            foreach (MeshRenderer item in blueprintObj.GetComponentsInChildren<MeshRenderer>())
                item.material = blueprintRed;
            doOnce = false;
        }
            
        else if(!obstructed && blueprintObj != null && !doOnce)
        {
            foreach (MeshRenderer item in blueprintObj.GetComponentsInChildren<MeshRenderer>())
                item.material = blueprintBlue;
            doOnce = true;
        }
    }

    void DeleteMachine()
    {
        if(Input.GetKey(KeyCode.LeftControl) && Input.GetMouseButtonDown(0))
        {
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
        if(blueprintObj != null && Input.GetMouseButton(0))
        {
            tagChecker = blueprintObj.GetComponent<BuildOnlyInTag>();
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
        if(Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1))
        {
            Destroy(blueprintObj);
            blueprintObj = null;
        }
    }

}
