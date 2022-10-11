using System.Collections;
using System;
using UnityEngine;

public class SideCheck : MonoBehaviour
{
    [Header("Sides")]
    public Transform front;
    public Transform back;
    public Transform left;
    public Transform right;

    public float boxSize;
    public bool ramp;
    Collider[] hitsFront;
    Collider[] hitsBack;
    Collider[] hitsLeft;
    Collider[] hitsRight;
    bool hitFront;
    bool hitBack;
    bool hitLeft;
    bool hitRight;
    public LayerMask machineLayer;
    public GameObject downRamp;
    public GameObject affectedSide;
    public GameObject affectedBridge;
    public Transform root;
    public bool isFront;

    [Space]
    [Header("Turns")]
    public GameObject leftTurn;
    public GameObject leftTurn_FrontLeft;
    public GameObject rightTurn;
    public GameObject rightTurn_FrontRight;
    public GameObject original;
    Conveyor cahcedItem;
    bool doOnce;

    void Start()
    {
        Invoke("LookForCurve",0.5f);
        // Invoke("CheckSide",1);
        // Invoke("CheckForMachineConnection",1);
        // if(isFront)
        //     Invoke("FrontCheck",1);
    }

    public void LookForCurve() {
        hitsFront = Physics.OverlapBox(front.position, new Vector3((boxSize / 2) * root.lossyScale.x, (boxSize * 1.2f) * root.lossyScale.y, (boxSize / 2)  * root.lossyScale.z), Quaternion.identity, machineLayer);
        hitsBack = Physics.OverlapBox(back.position, new Vector3((boxSize / 2) * root.lossyScale.x, (boxSize * 1.2f) * root.lossyScale.y, (boxSize / 2)  * root.lossyScale.z), Quaternion.identity, machineLayer);
        hitsRight = Physics.OverlapBox(right.position, new Vector3((boxSize / 2) * root.lossyScale.x, (boxSize * 1.2f) * root.lossyScale.y, (boxSize / 2)  * root.lossyScale.z), Quaternion.identity, machineLayer);
        hitsLeft = Physics.OverlapBox(left.position, new Vector3((boxSize / 2) * root.lossyScale.x, (boxSize * 1.2f) * root.lossyScale.y, (boxSize / 2)  * root.lossyScale.z), Quaternion.identity, machineLayer);
        foreach (Collider item in hitsFront)
            if (item.gameObject.CompareTag("Conveyor") && item != root.gameObject.GetComponent<Collider>()) {
                cahcedItem = item.gameObject.GetComponent<Conveyor>();
                hitFront = true;
            }

        foreach (Collider item in hitsBack)
            if (item.gameObject.CompareTag("Conveyor") && item != root.gameObject.GetComponent<Collider>()) {
                cahcedItem = item.gameObject.GetComponent<Conveyor>();
                hitBack = true;
            }
        
        foreach (Collider item in hitsRight)
            if (item.gameObject.CompareTag("Conveyor") && item != root.gameObject.GetComponent<Collider>()) {
                cahcedItem = item.gameObject.GetComponent<Conveyor>();
                hitRight = true;
            }

        foreach (Collider item in hitsLeft)
            if (item.gameObject.CompareTag("Conveyor") && item != root.gameObject.GetComponent<Collider>()) {
                cahcedItem = item.gameObject.GetComponent<Conveyor>();
                hitLeft = true;
            }

        
        if(hitFront && !doOnce) {
            Debug.Log("called");
            doOnce = true;
            leftTurn.SetActive(false);
            rightTurn.SetActive(false);
            original.SetActive(true);
            cahcedItem.GetComponentInChildren<SideCheck>().LookForCurve();
        }
        if(hitBack && !doOnce) {
            Debug.Log("called");
            doOnce = true;
            leftTurn.SetActive(false);
            rightTurn.SetActive(false);
            original.SetActive(true);
            cahcedItem.GetComponentInChildren<SideCheck>().LookForCurve();
        }
        if(hitRight && !hitFront && !hitBack && !hitLeft) {
            rightTurn.SetActive(true);
            rightTurn.transform.right = cahcedItem.transform.forward;
            
            leftTurn.SetActive(false);
            leftTurn_FrontLeft.SetActive(false);
            rightTurn_FrontRight.SetActive(false);
            
            original.SetActive(false);
        }
        else if(hitRight && hitFront && !hitBack && !hitLeft) {
            rightTurn.SetActive(true);
            rightTurn.transform.right = cahcedItem.transform.forward;
            
            leftTurn.SetActive(false);
            leftTurn_FrontLeft.SetActive(false);
            rightTurn_FrontRight.SetActive(false);
            
            original.SetActive(false);
        }
        else if(!hitRight && !hitFront && !hitBack && hitLeft) {
            leftTurn.SetActive(true);

            leftTurn.transform.right = cahcedItem.transform.forward;
            
            leftTurn_FrontLeft.SetActive(false);
            rightTurn.SetActive(false);
            rightTurn_FrontRight.SetActive(false);

            original.SetActive(false);
        }
        else if(!hitRight && hitFront && !hitBack && hitLeft) {
            leftTurn.SetActive(true);

            leftTurn.transform.right = cahcedItem.transform.forward;
            
            leftTurn_FrontLeft.SetActive(false);
            rightTurn.SetActive(false);
            rightTurn_FrontRight.SetActive(false);

            original.SetActive(false);
        }

        StartCoroutine(WaitDoOnce());

    }

    IEnumerator WaitDoOnce()
    {
        yield return new WaitForSeconds(1);
        doOnce = false;
    }

    public void CheckForMachineConnection()
    {
        hitsFront = Physics.OverlapBox(transform.position, new Vector3((boxSize / 2) * root.lossyScale.x, (boxSize / 2) * root.lossyScale.y, (boxSize / 2)  * root.lossyScale.z));

        foreach (Collider item in hitsFront)
        {
            if(item.gameObject.GetComponent<Machine>() != null)
            {
                if(affectedSide != null)
                    affectedSide.SetActive(false);
                if(affectedBridge != null)
                    affectedBridge.SetActive(true);
            }
        }
    }
}
