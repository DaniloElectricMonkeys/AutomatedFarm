using UnityEngine;
using DG.Tweening;

public class CameraManager : Singleton<CameraManager>
{
    #region Settings
    public enum CameraType { Ortographic, Perspective }
    public CameraType type;
    [Header("Ortographic")]
    [SerializeField] float minOrtoZoom = 5;
    [SerializeField] float maxOrtoZoom = 10;
    [Header("Perspective")]
    [SerializeField] float minPerspZoom = -5;
    [SerializeField] float maxPerspZoom = -20;
    [Header("Camera")]
    [SerializeField] float camSpeed = 50f;
    [Header("Rotation")]
    [SerializeField] float minRotAxisX = 20;
    [SerializeField] float maxRotAxisX = 50;
    [Header("Bounds")]
    [SerializeField] float minBoundsX = -30;
    [SerializeField] float maxBoundsX = 30;
    [SerializeField] float minBoundsZ = -30;
    [SerializeField] float maxBoundsZ = 30;
    readonly float[] Rotations = { 45, -45, -135 , 135 };
    int currentRotation;
    bool rotatingX;

    [Header("References")]
    [SerializeField] Camera myCamera;
    [SerializeField] Camera UICamera;
    public Transform helper;

    private void Start()
    {
        RotateCamera();
    }

    private void Update()
    {
        CameraMovement();        
        CameraRotate();        
        RotateCameraAxisX();
        SetCamera();
    }

    #endregion

    #region Functions

    void CameraMovement()
    {
        Vector3 inputDirection = Vector3.zero;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) inputDirection += helper.forward; // Forward
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) inputDirection -= helper.forward; // Back
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) inputDirection -= helper.right; // Left
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) inputDirection += helper.right; // Right

        transform.position += camSpeed * Time.deltaTime * inputDirection; // Move           
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, minBoundsX, maxBoundsX), transform.position.y, Mathf.Clamp(transform.position.z, minBoundsZ, maxBoundsZ));        
    }

    void OrtoZoom()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && myCamera.orthographicSize >= minOrtoZoom)
        {
            myCamera.DOOrthoSize(myCamera.orthographicSize - 1, 0.5f); // In
            UICamera.DOOrthoSize(UICamera.orthographicSize - 1, 0.5f); // In
        } 
        if (Input.GetAxis("Mouse ScrollWheel") < 0 && myCamera.orthographicSize <= maxOrtoZoom)
        {
            myCamera.DOOrthoSize(myCamera.orthographicSize + 1, 0.5f); // Out
            UICamera.DOOrthoSize(UICamera.orthographicSize + 1, 0.5f); // Out
        }
    }

    void PerspZoom()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && myCamera.transform.localPosition.z <= minPerspZoom)
        {
            myCamera.transform.DOLocalMoveZ(myCamera.transform.localPosition.z + 1, 0.3f); // In
            UICamera.transform.DOLocalMoveZ(UICamera.transform.localPosition.z + 1, 0.3f); // In
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0 && myCamera.transform.localPosition.z >= maxPerspZoom)
        {
            myCamera.transform.DOLocalMoveZ(myCamera.transform.localPosition.z - 1, 0.3f); // Out
            UICamera.transform.DOLocalMoveZ(UICamera.transform.localPosition.z - 1, 0.3f); // Out
        }
    }

    void CameraRotate()
    {
        if (Input.GetKeyUp(KeyCode.Q))
        {
            currentRotation--;
            RotateCamera();
        }
        if (Input.GetKeyUp(KeyCode.E))
        {
            currentRotation++;
            RotateCamera();
        }
    }

    void RotateCamera()
    {
        if (currentRotation < 0) currentRotation = Rotations.Length - 1;
        if (currentRotation > Rotations.Length - 1) currentRotation = 0;
        Quaternion finalRotation = Quaternion.Euler(transform.eulerAngles.x, Rotations[currentRotation], transform.eulerAngles.z);
        transform.DORotateQuaternion(finalRotation, 0.3f);
    }

    void RotateCameraAxisX()
    {        
        if (Input.GetKey(KeyCode.Z) && transform.eulerAngles.x < maxRotAxisX && !rotatingX)
        {
            rotatingX = true;
            Quaternion updateRotation = Quaternion.Euler(transform.eulerAngles.x + 5, transform.eulerAngles.y, transform.eulerAngles.z);
            Quaternion helperRotation = Quaternion.Euler(helper.eulerAngles.x - 5, helper.eulerAngles.y, helper.eulerAngles.z);
            transform.DORotateQuaternion(updateRotation, 0.3f);
            helper.DORotateQuaternion(helperRotation, 0.3f).OnComplete(() => rotatingX = false);
        }
        if (Input.GetKey(KeyCode.X) && transform.eulerAngles.x > minRotAxisX + 1 && !rotatingX)
        {
            rotatingX = true;
            Quaternion updateRotation = Quaternion.Euler(transform.eulerAngles.x - 5, transform.eulerAngles.y, transform.eulerAngles.z);
            Quaternion helperRotation = Quaternion.Euler(helper.eulerAngles.x + 5, helper.eulerAngles.y, helper.eulerAngles.z);
            transform.DORotateQuaternion(updateRotation, 0.3f);
            helper.DORotateQuaternion(helperRotation, 0.3f).OnComplete(() => rotatingX = false);
        }
    }

    void SetCamera()
    {
        if (type == CameraType.Ortographic)
        {
            myCamera.orthographic = true;
            UICamera.orthographic = true;
            OrtoZoom();
        }

        if (type == CameraType.Perspective)
        {
            myCamera.orthographic = false;
            UICamera.orthographic = false;
            PerspZoom();
        }
    }

    public void ToggleCamera()
    {
        switch (type)
        {
            case CameraType.Ortographic:
                type = CameraType.Perspective;
                myCamera.transform.DOLocalMoveZ(maxPerspZoom, 0.2f);
                UICamera.transform.DOLocalMoveZ(maxPerspZoom, 0.2f);
                break;
            case CameraType.Perspective:
                myCamera.transform.DOLocalMoveZ(maxPerspZoom, 0);
                UICamera.transform.DOLocalMoveZ(maxPerspZoom, 0);
                type = CameraType.Ortographic;
                myCamera.DOOrthoSize(maxOrtoZoom, 0.2f);
                UICamera.DOOrthoSize(maxOrtoZoom, 0.2f);
                break;
        }
    }
    #endregion
}
