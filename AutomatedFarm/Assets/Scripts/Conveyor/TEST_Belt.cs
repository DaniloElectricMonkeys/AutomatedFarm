using System.Collections;
using UnityEngine;

public class TEST_Belt : MonoBehaviour
{
    private static int _beltID = 0;

    [Header("Attributes")]
    public TEST_Belt beltInSequence;
    public TEST_BeltItem beltItem;
    public bool isSpaceTaken;
    public OutputMachine selectedMachine;
    private TEST_BeltManager _beltManager;
    public bool input;
    public bool output;
    public int inputDoor = 0;
    bool doOnce;
    [Space]
    [Header("Evade Belts")]
    public TEST_Belt[] avoidThisBelts;

    [Space]
    [Header("Materials")]
    public MeshRenderer meshRenderer;
    public Material movingMat;
    public Material stopedMat;

    float timeStoped;
    [HideInInspector] public bool isRampUp;
    [HideInInspector] public bool isRampDown;
    public GameObject rampNormal;

    private void Start()
    {
        _beltManager = FindObjectOfType<TEST_BeltManager>();
        beltInSequence = null;
        beltInSequence = FindNextBelt();
        gameObject.name = $"Belt: {_beltID++}";
    }

    private void Update()
    {
        if(selectedMachine == null)
            selectedMachine = FindMachine();

        if(selectedMachine != null && beltItem == null && isSpaceTaken == false && output)
        {
            if(selectedMachine.GetResourceAmount() > 0)
            {
                beltItem = selectedMachine.AskForBeltItem();
                // if(beltItem != null)
                //     StartCoroutine(StartBeltMove());
            }
        }

        if (beltInSequence == null)
            beltInSequence = FindNextBelt();

        if (beltItem != null && beltItem.item != null)
            StartCoroutine(StartBeltMove());
    }

    public Vector3 GetItemPosition()
    {
        var padding = 0.9f;

        if(isRampUp) padding = 1.1f;
        if(isRampDown) padding = 1.1f;

        var position = transform.position;
        return new Vector3(position.x, position.y + padding, position.z);
    }

    private IEnumerator StartBeltMove()
    {
        isSpaceTaken = true;

        if (beltItem.item != null && beltInSequence != null && beltInSequence.isSpaceTaken == false)
        {
            meshRenderer.material = movingMat;
            timeStoped = 0;

            Vector3 toPosition = beltInSequence.GetItemPosition();
            beltItem.currentConveyor = beltInSequence;

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
        else if (beltItem.item != null && beltInSequence == null && selectedMachine != null && input && doOnce == false && selectedMachine.IsInventoryFull(beltItem.type, inputDoor) == false)
        {
            meshRenderer.material = movingMat;
            timeStoped = 0;
            doOnce = true;
            Vector3 toPosition = GetItemPosition() + transform.forward;

            var step = _beltManager.speed * Time.deltaTime;

            while (beltItem.item.transform.position != toPosition)
            {
                beltItem.item.transform.position = 
                    Vector3.MoveTowards(beltItem.transform.position, toPosition, step);

                yield return null;
            }

            selectedMachine.OnResourceEnter(beltItem.type, beltItem.item, 1);

            isSpaceTaken = false;
            beltItem = null;
            doOnce = false;
        }
        else
        {
            timeStoped += Time.deltaTime;
            if(timeStoped >= 1f)
            {
                meshRenderer.material = stopedMat;
            }
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
            
            if(belt == this) return null;
            
            for (int i = 0; i < avoidThisBelts.Length; i++)
                if(belt == avoidThisBelts[i]) return null;

            if (belt != null)
                return belt;
        }

        return null;
    }

    private OutputMachine FindMachine()
    {
        Transform currentBeltTransform = transform;
        RaycastHit hit;

        Ray rayForward = new Ray(currentBeltTransform.position, transform.forward);
        Ray rayBackwards = new Ray(currentBeltTransform.position, -transform.forward);

        if (Physics.Raycast(rayForward, out hit, 1f))
        {
            OutputMachine machine = hit.collider.GetComponent<OutputMachine>();

            if (machine != null)
            {
                input = true;
                return machine;
            }
        }

        if (Physics.Raycast(rayBackwards, out hit, 1f))
        {
            OutputMachine machine = hit.collider.GetComponent<OutputMachine>();

            if (machine != null)
            {
                output = true;
                return machine;
            }
        }

        return null;
    }

    private void OnDrawGizmos() {
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 1f);
        Gizmos.DrawLine(transform.position, transform.position + (-transform.forward * 1f));
    }
}