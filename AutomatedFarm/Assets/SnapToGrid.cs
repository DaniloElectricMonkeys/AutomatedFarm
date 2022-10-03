using UnityEngine;

public class SnapToGrid : MonoBehaviour
{
    public bool parentLock;
    private void LateUpdate()
    {
        if(parentLock == true)
            transform.position = NewGrid.instance.GetGridPoint(transform.parent.position);
        else
            transform.position = NewGrid.instance.GetGridPoint(transform.position);
    }
}
