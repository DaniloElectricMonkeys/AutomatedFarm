using System.Collections;
using System;
using UnityEngine;

public class SideCheck : MonoBehaviour
{
    public SideCheck upRamp;
    public SideCheck downRamp;
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
    public Transform rampRoot;

    [Space]
    [Header("Turns & Ramps")]
    public GameObject rampObj;
    public GameObject rampObjUp;
    public GameObject leftTurn;
    public GameObject leftTurn_FrontLeft;
    public GameObject rightTurn;
    public GameObject rightTurn_FrontRight;
    public GameObject original;
    public GameObject originalCollider;
    public GameObject rampNormal;
    TEST_Belt itemThatWasHit;
    public bool isTurn;
    public bool doNotUpdate;
    [Space]
    [Header("End Point")]
    TEST_Belt thisConveyor;

    private void Awake() {
        thisConveyor = GetComponentInParent<TEST_Belt>();
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
                itemThatWasHit = item.gameObject.GetComponent<TEST_Belt>();
                hitFront = true;
            }

        foreach (Collider item in hitsBack)
            if (item.gameObject.CompareTag("Conveyor") && item != root.gameObject.GetComponent<Collider>()) {
                itemThatWasHit = item.gameObject.GetComponent<TEST_Belt>();
                hitBack = true;
            }
        
        foreach (Collider item in hitsRight)
            if (item.gameObject.CompareTag("Conveyor") && item != root.gameObject.GetComponent<Collider>()) {
                itemThatWasHit = item.gameObject.GetComponent<TEST_Belt>();
                hitRight = true;
            }

        foreach (Collider item in hitsLeft)
            if (item.gameObject.CompareTag("Conveyor") && item != root.gameObject.GetComponent<Collider>()) {
                itemThatWasHit = item.gameObject.GetComponent<TEST_Belt>();
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
            originalCollider.SetActive(false);
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
            originalCollider.SetActive(false);
        }
    }

    public void EnableRampUp()
    {
        rightTurn.SetActive(false);
        leftTurn.SetActive(false);
        leftTurn_FrontLeft.SetActive(false);
        rightTurn_FrontRight.SetActive(false);
        original.SetActive(false);
        originalCollider.SetActive(false);

        rampObjUp.SetActive(true);
        root.gameObject.GetComponent<TEST_Belt>().isRampUp = true;
        root.gameObject.GetComponent<TEST_Belt>().rampNormal = upRamp.rampNormal;
    }

    public void EnableRampDown() {
        rightTurn.SetActive(false);
        leftTurn.SetActive(false);
        leftTurn_FrontLeft.SetActive(false);
        rightTurn_FrontRight.SetActive(false);
        original.SetActive(false);
        originalCollider.SetActive(false);

        rampObj.SetActive(true);
        root.gameObject.GetComponent<TEST_Belt>().isRampDown = true;
        root.gameObject.GetComponent<TEST_Belt>().rampNormal = downRamp.rampNormal;
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
