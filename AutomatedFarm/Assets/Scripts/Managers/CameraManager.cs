using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] float camSpeed = 50f; // Camera speed

    private void Update()
    {
        Vector3 inputDirection = Vector3.zero;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) inputDirection += Vector3.right; // Forward
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) inputDirection -= Vector3.right; // Back
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) inputDirection += Vector3.forward; // Left
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) inputDirection -= Vector3.forward; // Right

        transform.position += inputDirection * camSpeed * Time.deltaTime; // Move
    }
}
