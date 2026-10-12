using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float lookSpeed = 0.1f;
    [SerializeField] private float jumpForce = 8f;
    [SerializeField] private float gravity = 20f;

    private CharacterController controller;
    private Vector3 moveDirection = Vector3.zero;
    private float rotationX = 0;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Rotation (Mouse Look)
        Vector2 mouseDelta = Mouse.current.delta.ReadValue();
        float mouseX = mouseDelta.x * lookSpeed;
        float mouseY = mouseDelta.y * lookSpeed;

        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -90f, 90f);

        Camera cam = GetComponentInChildren<Camera>();
        if (cam != null) cam.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.Rotate(Vector3.up * mouseX);

        // Movement (WASD)
        Vector2 moveInput = Vector2.zero;
        if (Keyboard.current != null)
        {
            float moveX = 0;
            float moveZ = 0;
            if (Keyboard.current.wKey.isPressed) moveZ = 1;
            if (Keyboard.current.sKey.isPressed) moveZ = -1;
            if (Keyboard.current.aKey.isPressed) moveX = -1;
            if (Keyboard.current.dKey.isPressed) moveX = 1;
            moveInput = new Vector2(moveX, moveZ);
        }

        if (controller.isGrounded)
        {
            moveDirection = new Vector3(moveInput.x, 0, moveInput.y);
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= moveSpeed;

            if (Keyboard.current != null && Keyboard.current.spaceKey.isPressed)
            {
                moveDirection.y = jumpForce;
            }
        }

        moveDirection.y -= gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);
    }
}
