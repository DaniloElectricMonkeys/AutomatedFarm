using System.Security.AccessControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyEnums;

public class BuildOnlyInTag : MonoBehaviour
{
    public Transform checkPoint;
    public LayerMask buildLayer;
    public new string tag = "";
    ResourceNode resNode;

    public bool CastCheckRay()
    {
        if(Physics.Raycast(NewGrid.instance.GetGridPoint(checkPoint.position), Vector3.down, out RaycastHit hit, 100f, buildLayer))
        {
            Debug.DrawRay(NewGrid.instance.GetGridPoint(checkPoint.position), Vector3.down * 100, Color.magenta, 10);
            if(hit.collider.gameObject.CompareTag(tag))
            {
                resNode = hit.collider.gameObject.GetComponent<ResourceNode>();
                return true;
            }
                
        }
        return false;
    }

    public ResourceType GetResourceBelow()
    {
        return resNode != null ? resNode.type : ResourceType.none;
    }
}
