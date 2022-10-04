using UnityEngine;
using DG.Tweening;

public class CameraManager : MonoBehaviour
{
    #region Settings

    [SerializeField] float camSpeed = 50f;
    [SerializeField] Camera myCamera;
    readonly float[] Rotations = { 45, 125, 225, 305 };
    int currentRotation;
    public Transform helper;

    private void Start()
    {
        RotateCamera();
    }

    private void Update()
    {
        CameraMovement();
    }

    #endregion

    #region Functions

    void CameraMovement()
    {
        #region Keyboard Movement
        Vector3 inputDirection = Vector3.zero;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) inputDirection += helper.forward; // Forward
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) inputDirection -= helper.forward; // Back
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) inputDirection -= helper.right; // Left
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) inputDirection += helper.right; // Right

        transform.position += camSpeed * Time.deltaTime * inputDirection; // Move
        #endregion

        #region Mouse Scroll Zoom
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && myCamera.orthographicSize > 5) myCamera.orthographicSize -= 1;
        if (Input.GetAxis("Mouse ScrollWheel") < 0 && myCamera.orthographicSize < 10) myCamera.orthographicSize += 1;
        #endregion

        #region Keyboard Rotation
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
        #endregion        
    }

    void RotateCamera()
    {
        if (currentRotation < 0) currentRotation = Rotations.Length - 1;
        if (currentRotation > Rotations.Length - 1) currentRotation = 0;
        Quaternion finalRotation = Quaternion.Euler(transform.eulerAngles.x, Rotations[currentRotation], transform.eulerAngles.z);
        transform.DORotateQuaternion(finalRotation, 0.3f);
        //transform.rotation = Quaternion.Euler(transform.eulerAngles.x, Rotations[currentRotation], transform.eulerAngles.z);
    }

    #endregion
}
