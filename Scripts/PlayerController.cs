using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script should be placed on an empty object with a character controller
// The object should have the main camera and a capsule as its children
// Put the camera in the camera transform slot
public class PlayerController : MonoBehaviour
{
    private float playerSpeed = 6.0f;
    private float floatSpeed = 2.0f;
    private float lookSpeed = 1.8f;
    private int cameraClamp = 60;
    Vector3 moveDirection, forward, right;
    float curSpeedFB, curSpeedLR, rotationVerticalCamera;
    public Transform cameraTransform;
    CharacterController characterController;
    LoT LensScript;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        rotationVerticalCamera = 0.0f;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        LensScript = GameObject.Find("LensOfTruth").GetComponent<LoT>();
    }

    void Update()
    {
        Movement();
        Camera();
        Interact();
    }

    private void Movement() {
        // Transform the local "forward" into global "forward"
        forward = transform.TransformDirection(Vector3.forward); // (0,0,1)
        right = transform.TransformDirection(Vector3.right); // (1,0,0)

        // WASD inputs
        curSpeedFB = playerSpeed * Input.GetAxis("Vertical"); // Forward-back
        curSpeedLR = playerSpeed * Input.GetAxis("Horizontal"); // Left-right

        moveDirection = (forward * curSpeedFB) + (right * curSpeedLR); // Combine curSpeeds into one vector
        characterController.Move(moveDirection * Time.deltaTime); // Move the player

        // Floating
        if (Input.GetKey("space")) {
            characterController.Move(floatSpeed * Vector3.up * Time.deltaTime);
        }
        if (Input.GetKey("left shift")) {
            characterController.Move(floatSpeed * Vector3.down * Time.deltaTime);
        }
    }

    private void Camera() {
        // Horizontal rotation (about y-axis)
        transform.Rotate(0, Input.GetAxis("Mouse X") * lookSpeed, 0);

        // Vertical rotation
        rotationVerticalCamera -= Input.GetAxis("Mouse Y") * lookSpeed;
        rotationVerticalCamera = Mathf.Clamp(rotationVerticalCamera, -cameraClamp, cameraClamp);
        cameraTransform.localRotation = Quaternion.Euler(rotationVerticalCamera, 0, 0); // this will G-lock if not local
    }

    private void Interact()
    {
        if (Input.GetKeyDown("e")) {
            LensScript.LensActivate();
        }
    }

}
