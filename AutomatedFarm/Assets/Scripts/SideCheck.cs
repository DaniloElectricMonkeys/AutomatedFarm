using UnityEngine;

public class SideCheck : MonoBehaviour
{
    public float boxSize;
    Collider[] hits;
    public GameObject affectedSide;
    public GameObject affectedBridge;
    public Transform root;
    public bool front;

    void Start()
    {
        Invoke("CheckSide",1);
        Invoke("CheckForMachineConnection",1);
        if(front)
            Invoke("FrontCheck",1);
    }

    private void CheckSide()
    {
        hits = Physics.OverlapBox(transform.position, new Vector3((boxSize / 2) * root.lossyScale.x, (boxSize / 2) * root.lossyScale.y, (boxSize / 2)  * root.lossyScale.z));

        foreach (Collider item in hits)
        {
            if (item.gameObject.CompareTag("Conveyor") && item != root.gameObject.GetComponent<Collider>())
            {
                if(affectedSide != null)
                    affectedSide.SetActive(false);
            }
        }
    }

    private void FrontCheck()
    {
        foreach (Collider item in hits)
        {
            if (item.gameObject.CompareTag("Conveyor") && item != root.gameObject.GetComponent<Collider>())
            {
                var sides = item.gameObject.GetComponentsInChildren<SideCheck>();

                for (int i = 0; i < sides.Length; i++)
                {
                    sides[i].CheckSide();
                }
            }
        }
    }

    public void CheckForMachineConnection()
    {
        hits = Physics.OverlapBox(transform.position, new Vector3((boxSize / 2) * root.lossyScale.x, (boxSize / 2) * root.lossyScale.y, (boxSize / 2)  * root.lossyScale.z));

        foreach (Collider item in hits)
        {
            if(item.gameObject.GetComponent<SingleMiner>() != null)
            {
                if(affectedSide != null)
                    affectedSide.SetActive(false);
                if(affectedBridge != null)
                    affectedBridge.SetActive(true);
            }
        }
    }

    private void OnDrawGizmos() 
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(boxSize * root.lossyScale.x,boxSize * root.lossyScale.y,boxSize * root.lossyScale.z));
    }
}
