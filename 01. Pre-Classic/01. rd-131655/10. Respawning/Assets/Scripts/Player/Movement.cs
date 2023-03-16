using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {    
    private CharacterController characterController;

    private Vector3 velocity;
    private float fallSpeed = -78.4f;

    [SerializeField] private bool isGrounded;

    private float jumpHeight = 1.2522f;

    private float speed = 4.317f;

    private Camera cam;

    private void Awake() {
        characterController = GetComponent<CharacterController>();
        cam = GetComponentInChildren<Camera>();
    }
    
    private void Start() {
        
    }

    private void Update() {
        FallUpdate();
        JumpUpdate();
        MovementUpdate();

        Cursor.lockState = CursorLockMode.Locked;
        CameraUpdate();

        RespawnUpdate();
    }

    private void FallUpdate() {
        velocity.y += fallSpeed * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
        
        isGrounded = characterController.isGrounded;

        if(isGrounded && velocity.y < 0) {
            velocity.y = -2.0f;
        }
    }

    private void JumpUpdate() {
        if(isGrounded && Input.GetButton("Space")) {
            isGrounded = false;

            velocity.y = Mathf.Sqrt(jumpHeight * -2.0f * fallSpeed);
        }
    }

    private void MovementUpdate() {
        float x = Input.GetAxis("HorizontalAD");
        float z = Input.GetAxis("VerticalWS");

        Vector3 moveDirection = transform.TransformDirection(new Vector3(x, 0.0f, z));
        moveDirection *= speed;
        characterController.Move(moveDirection * Time.deltaTime);
    }

    private void CameraUpdate() {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        transform.Rotate(Vector3.up * mouseX);

        float xRotation = 0;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90, 90);

        cam.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
    }

    private void RespawnUpdate() {
        Vector3 WorldPosition = new Vector3(
            World.WorldSizeInVoxels.x / 2,
            World.WorldSizeInVoxels.y,
            World.WorldSizeInVoxels.z / 2
        );
        
        float x = Random.Range(-WorldPosition.x, WorldPosition.x);
        float y = WorldPosition.y;
        float z = Random.Range(-WorldPosition.z, WorldPosition.z);

        Vector3 respawn = new Vector3(x, y, z);

        if(Input.GetKey(KeyCode.R)) {
            characterController.transform.position = respawn;
        }
    }
}
