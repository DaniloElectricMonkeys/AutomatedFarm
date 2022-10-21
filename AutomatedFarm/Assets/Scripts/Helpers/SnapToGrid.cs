using UnityEngine;

public class SnapToGrid : MonoBehaviour
{
    public bool parentLock;
    private void LateUpdate()
    {
        if(parentLock == true)
            transform.position = NewGrid.Instance.GetGridPoint(transform.parent.position);
        else
            transform.position = NewGrid.Instance.GetGridPoint(transform.position);
    }
}
