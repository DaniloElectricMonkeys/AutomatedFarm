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

    public Vector3 boxSize;
    Collider[] hitsFront;
    Collider[] hitsBack;
    Collider[] hitsLeft;
    Collider[] hitsRight;
    bool hitFront;
    bool hitBack;
    bool hitLeft;
    bool hitRight;
    public LayerMask machineLayer;
    public Transform root;

    [Space]
    [Header("Turns & Ramps")]
    public GameObject rampObj;
    public GameObject rampObjUp;
    public GameObject leftTurn;
    public GameObject leftTurn_FrontLeft;
    public GameObject rightTurn;
    public GameObject rightTurn_FrontRight;
    public GameObject original;
    Conveyor itemThatWasHit;
    public bool isTurn;
    public bool doNotUpdate;
    [Space]
    [Header("End Point")]
    public Transform endPoint;
    Conveyor thisConveyor;

    private void Awake() {
        thisConveyor = GetComponentInParent<Conveyor>();
    }
    void Start()
    {
        Invoke("LookForCurve",0.5f);
        // Invoke("CheckSide",1);
        // Invoke("CheckForMachineConnection",1);
        // if(isFront)
        //     Invoke("FrontCheck",1);
    }

    public void LookForCurve() {

        if(doNotUpdate) return;

        hitsFront = Physics.OverlapBox(front.position, new Vector3((boxSize.x / 2) * root.lossyScale.x, (boxSize.y * 1.2f) * root.lossyScale.y, (boxSize.z / 2)  * root.lossyScale.z), Quaternion.identity, machineLayer);
        hitsBack = Physics.OverlapBox(back.position, new Vector3((boxSize.x / 2) * root.lossyScale.x, (boxSize.y * 1.2f) * root.lossyScale.y, (boxSize.z / 2)  * root.lossyScale.z), Quaternion.identity, machineLayer);
        hitsRight = Physics.OverlapBox(right.position, new Vector3((boxSize.x / 2) * root.lossyScale.x, (boxSize.y * 1.2f) * root.lossyScale.y, (boxSize.z / 2)  * root.lossyScale.z), Quaternion.identity, machineLayer);
        hitsLeft = Physics.OverlapBox(left.position, new Vector3((boxSize.x / 2) * root.lossyScale.x, (boxSize.y * 1.2f) * root.lossyScale.y, (boxSize.z / 2)  * root.lossyScale.z), Quaternion.identity, machineLayer);
        foreach (Collider item in hitsFront)
            if (item.gameObject.CompareTag("Conveyor") && item != root.gameObject.GetComponent<Collider>()) {
                itemThatWasHit = item.gameObject.GetComponent<Conveyor>();
                hitFront = true;
            }

        foreach (Collider item in hitsBack)
            if (item.gameObject.CompareTag("Conveyor") && item != root.gameObject.GetComponent<Collider>()) {
                itemThatWasHit = item.gameObject.GetComponent<Conveyor>();
                hitBack = true;
            }
        
        foreach (Collider item in hitsRight)
            if (item.gameObject.CompareTag("Conveyor") && item != root.gameObject.GetComponent<Collider>()) {
                itemThatWasHit = item.gameObject.GetComponent<Conveyor>();
                hitRight = true;
            }

        foreach (Collider item in hitsLeft)
            if (item.gameObject.CompareTag("Conveyor") && item != root.gameObject.GetComponent<Collider>()) {
                itemThatWasHit = item.gameObject.GetComponent<Conveyor>();
                hitLeft = true;
            }

        if(hitBack) {
            if( itemThatWasHit.transform.position.y < thisConveyor.transform.position.y) {
                itemThatWasHit.GetComponentInChildren<SideCheck>().EnableRampUp();
            }
            else if( itemThatWasHit.transform.position.y > thisConveyor.transform.position.y)
                EnableRampDown();
        }
        else if(hitFront) {
            if(itemThatWasHit.transform.position.y > thisConveyor.transform.position.y)
                EnableRampUp();
            else if(itemThatWasHit.transform.position.y < thisConveyor.transform.position.y)
                itemThatWasHit.GetComponentInChildren<SideCheck>().EnableRampDown();
        }

        if(hitRight && !hitLeft) {

            if(hitRight && hitBack)// If we hit someting in thosse positions
                if(itemThatWasHit.GetComponentInChildren<SideCheck>().isTurn) // Check if is a turn and return.
                    return;
                    
            rightTurn.SetActive(true);
            isTurn = true;
            rightTurn.transform.right = itemThatWasHit.transform.forward;
            
            leftTurn.SetActive(false);
            leftTurn_FrontLeft.SetActive(false);
            rightTurn_FrontRight.SetActive(false);
            original.SetActive(false);
        }
        else if(!hitRight && hitLeft) {

            if(hitLeft && hitBack)// If we hit someting in thosse positions
                if(itemThatWasHit.GetComponentInChildren<SideCheck>().isTurn) // Check if is a turn and return.
                    return;
                    
            leftTurn.SetActive(true);
            isTurn = true;
            leftTurn.transform.right = itemThatWasHit.transform.forward;
            
            leftTurn_FrontLeft.SetActive(false);
            rightTurn.SetActive(false);
            rightTurn_FrontRight.SetActive(false);
            original.SetActive(false);
        }
        

        
        // if(hitFront && !doOnce) {
        //     doOnce = true;
        //     leftTurn.SetActive(false);
        //     rightTurn.SetActive(false);
        //     if(!rampObj.activeSelf || !rampObj.activeSelf)
        //         original.SetActive(true);
            
        //     itemThatWasHit.GetComponentInChildren<SideCheck>().LookForCurve();
        // }
        // if(hitBack && !doOnce) {
        //     doOnce = true;
        //     leftTurn.SetActive(false);
        //     rightTurn.SetActive(false);
        //     if(!rampObj.activeSelf || !rampObj.activeSelf)
        //         original.SetActive(true);
        //     itemThatWasHit.GetComponentInChildren<SideCheck>().LookForCurve();
        // }
        // if(!hitRight && !hitFront && hitBack && !hitLeft) {

        //     rightTurn.SetActive(false);
        //     leftTurn.SetActive(false);
        //     leftTurn_FrontLeft.SetActive(false);
        //     rightTurn_FrontRight.SetActive(false);
        //     original.SetActive(false);

        //     if(itemThatWasHit.transform.position.y > transform.position.y)
        //         rampObj.SetActive(true);
        //     else if(itemThatWasHit.transform.position.y < transform.position.y) {
        //         original.SetActive(true);
        //         itemThatWasHit.GetComponentInChildren<SideCheck>().LookForCurve();
        //     }
        //     else
        //         original.SetActive(true);
        // }
        // else if(!hitRight && hitFront && !hitBack && !hitLeft) {

        //     if(itemThatWasHit.transform.position.y > transform.position.y)
        //         EnableRampUp();
        //     else
        //         original.SetActive(true);
        // }
        // else if(hitRight && !hitFront && !hitBack && !hitLeft) {
        //     rightTurn.SetActive(true);
        //     rightTurn.transform.right = itemThatWasHit.transform.forward;
            
        //     leftTurn.SetActive(false);
        //     leftTurn_FrontLeft.SetActive(false);
        //     rightTurn_FrontRight.SetActive(false);
            
        //     original.SetActive(false);
        // }
        // else if(hitRight && hitFront && !hitBack && !hitLeft) {
        //     rightTurn.SetActive(true);
        //     rightTurn.transform.right = itemThatWasHit.transform.forward;
            
        //     leftTurn.SetActive(false);
        //     leftTurn_FrontLeft.SetActive(false);
        //     rightTurn_FrontRight.SetActive(false);
            
        //     original.SetActive(false);
        // }
        // else if(!hitRight && !hitFront && !hitBack && hitLeft) {
        //     leftTurn.SetActive(true);

        //     leftTurn.transform.right = itemThatWasHit.transform.forward;
            
        //     leftTurn_FrontLeft.SetActive(false);
        //     rightTurn.SetActive(false);
        //     rightTurn_FrontRight.SetActive(false);

        //     original.SetActive(false);
        // }
        // else if(!hitRight && hitFront && !hitBack && hitLeft) {
        //     leftTurn.SetActive(true);

        //     leftTurn.transform.right = itemThatWasHit.transform.forward;
            
        //     leftTurn_FrontLeft.SetActive(false);
        //     rightTurn.SetActive(false);
        //     rightTurn_FrontRight.SetActive(false);

        //     original.SetActive(false);
        // }

        // StartCoroutine(WaitDoOnce());

    }

    // IEnumerator WaitDoOnce()
    // {
    //     yield return new WaitForSeconds(1);
    //     doOnce = false;
    // }

    public void EnableRampUp()
    {
        rightTurn.SetActive(false);
        leftTurn.SetActive(false);
        leftTurn_FrontLeft.SetActive(false);
        rightTurn_FrontRight.SetActive(false);
        original.SetActive(false);

        rampObjUp.SetActive(true);
        endPoint.position = new Vector3(0, 1.44f, 0.76f);
    }

    public void EnableRampDown() {
        rightTurn.SetActive(false);
        leftTurn.SetActive(false);
        leftTurn_FrontLeft.SetActive(false);
        rightTurn_FrontRight.SetActive(false);
        original.SetActive(false);

        rampObj.SetActive(true);
    }

    public void CheckForMachineConnection()
    {
        // hitsFront = Physics.OverlapBox(transform.position, new Vector3((boxSize / 2) * root.lossyScale.x, (boxSize / 2) * root.lossyScale.y, (boxSize / 2)  * root.lossyScale.z));

        // foreach (Collider item in hitsFront)
        // {
        //     if(item.gameObject.GetComponent<Machine>() != null)
        //     {
        //         if(affectedSide != null)
        //             affectedSide.SetActive(false);
        //         if(affectedBridge != null)
        //             affectedBridge.SetActive(true);
        //     }
        // }
    }
}
