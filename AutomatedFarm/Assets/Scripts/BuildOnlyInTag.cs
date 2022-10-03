using System.Security.AccessControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyEnums;

public class BuildOnlyInTag : MonoBehaviour
{
    // This class check the current tag and layer below the blueprint object
    // If tag and layer are apropriated, build is aproved.

    public Transform checkPoint;
    public LayerMask buildLayer;
    public new string tag = "";
    ResourceNode resNode;


    ///<summary>
    /// Return true if raycast hit a resource node.
    ///<summary>
    public bool RayHitNode()
    {
        if(Physics.Raycast(NewGrid.Instance.GetGridPoint(checkPoint.position), Vector3.down, out RaycastHit hit, 100f, buildLayer))
        {
            Debug.DrawRay(NewGrid.Instance.GetGridPoint(checkPoint.position), Vector3.down * 100, Color.magenta, 10);
            if(hit.collider.gameObject.CompareTag(tag))
            {
                resNode = hit.collider.gameObject.GetComponent<ResourceNode>();
                if(resNode != null)
                    return true;
                else
                    return false;
            }
                
        }
        return false;
    }

    public ResourceType GetResourceBelow()
    {
        return resNode != null ? resNode.type : ResourceType.none;
    }
}
