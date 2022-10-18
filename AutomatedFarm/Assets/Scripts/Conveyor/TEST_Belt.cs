using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST_Belt : MonoBehaviour
{
    private static int _beltID = 0;

    public TEST_Belt beltInSequence;
    public TEST_BeltItem beltItem;
    public bool isSpaceTaken;

    private TEST_BeltManager _beltManager;

    private void Start()
    {
        _beltManager = FindObjectOfType<TEST_BeltManager>();
        beltInSequence = null;
        beltInSequence = FindNextBelt();
        gameObject.name = $"Belt: {_beltID++}";
    }

    private void Update()
    {
        if (beltInSequence == null)
            beltInSequence = FindNextBelt();

        if (beltItem != null && beltItem.item != null)
            StartCoroutine(StartBeltMove());
    }

    public Vector3 GetItemPosition()
    {
        var padding = 1f;
        var position = transform.position;
        return new Vector3(position.x, position.y + padding, position.z);
    }

    private IEnumerator StartBeltMove()
    {
        isSpaceTaken = true;

        if (beltItem.item != null && beltInSequence != null && beltInSequence.isSpaceTaken == false)
        {
            Vector3 toPosition = beltInSequence.GetItemPosition();

            beltInSequence.isSpaceTaken = true;

            var step = _beltManager.speed * Time.deltaTime;

            while (beltItem.item.transform.position != toPosition)
            {
                beltItem.item.transform.position = 
                    Vector3.MoveTowards(beltItem.transform.position, toPosition, step);

                yield return null;
            }

            isSpaceTaken = false;
            beltInSequence.beltItem = beltItem;
            beltItem = null;
        }
    }

    private TEST_Belt FindNextBelt()
    {
        Transform currentBeltTransform = transform;
        RaycastHit hit;

        var forward = transform.forward;

        Ray ray = new Ray(currentBeltTransform.position, forward);
        if (Physics.Raycast(ray, out hit, 1f))
        {
            TEST_Belt belt = hit.collider.GetComponent<TEST_Belt>();

            if (belt != null)
                return belt;
        }

        return null;
    }

    private void OnDrawGizmos() {
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 1f);
    }
}