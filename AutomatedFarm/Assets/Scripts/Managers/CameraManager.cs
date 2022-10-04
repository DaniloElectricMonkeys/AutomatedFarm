using UnityEngine;
using DG.Tweening;

public class CameraManager : MonoBehaviour
{
    #region Settings

    [Header("Settings")]
    [SerializeField] float minZoom = 5;
    [SerializeField] float maxZoom = 10;
    [SerializeField] float camSpeed = 50f;
    [SerializeField] float minBoundsX = -30;
    [SerializeField] float maxBoundsX = 30;
    [SerializeField] float minBoundsZ = -30;
    [SerializeField] float maxBoundsZ = 30;
    readonly float[] Rotations = { 45, -45, -135 , 135 };
    int currentRotation;

    [Header("References")]
    [SerializeField] Camera myCamera;
    public Transform helper;

    private void Start()
    {
        RotateCamera();
    }

    private void Update()
    {
        CameraMovement();
        CameraZoom();
        CameraRotate();
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

    void CameraZoom()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && myCamera.orthographicSize > minZoom) myCamera.orthographicSize -= 1; // In
        if (Input.GetAxis("Mouse ScrollWheel") < 0 && myCamera.orthographicSize < maxZoom) myCamera.orthographicSize += 1; // Out
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

    #endregion
}
