using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 6f;
    public float sprintSpeed = 10f;
    public float acceleration = 40f;
    public float jumpForce = 5f;

    [Header("Look Settings")]
    public Transform cameraHolder;
    public float mouseSensitivity = 0.1f;
    public float lookSmoothTime = 0.03f;
    public float minPitch = -85f;
    public float maxPitch = 85f;

    [Header("Ground Check")]
    public float groundDistance = 0.2f;
    public LayerMask groundLayer;

    private Rigidbody rb;
    private Collider col;
    private Vector2 moveInput;
    private Vector2 targetLookDelta;
    private Vector2 currentLookDelta;
    private Vector2 lookSmoothVel;
    private float pitch;
    private float yaw;
    private bool jumpRequested;
    private bool isSprinting;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        rb.freezeRotation = true;
        // ponytail: interpolation smooth render frame
        rb.interpolation = RigidbodyInterpolation.Interpolate;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        yaw = transform.eulerAngles.y;

        if (cameraHolder == null && Camera.main != null)
        {
            cameraHolder = Camera.main.transform;
        }

        if (cameraHolder != null)
        {
            pitch = cameraHolder.localEulerAngles.x;
            if (pitch > 180f) pitch -= 360f;
        }
    }

    void Update()
    {
        HandleInput();
        HandleLook();
    }

    void FixedUpdate()
    {
        CheckGrounded();
        HandleMovement();
    }

    void HandleInput()
    {
        if (Keyboard.current == null) return;

        float moveX = 0f;
        float moveZ = 0f;

        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) moveX -= 1f;
        if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) moveX += 1f;
        if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) moveZ -= 1f;
        if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) moveZ += 1f;

        moveInput = new Vector2(moveX, moveZ).normalized;
        isSprinting = Keyboard.current.leftShiftKey.isPressed;

        if (Keyboard.current.spaceKey.wasPressedThisFrame && isGrounded)
        {
            jumpRequested = true;
        }

        if (Mouse.current != null)
        {
            targetLookDelta = Mouse.current.delta.ReadValue() * mouseSensitivity;
        }
        else
        {
            targetLookDelta = Vector2.zero;
        }
    }

    void HandleLook()
    {
        // Smooth input delta untuk hilangkan jitter mouse polling rate
        currentLookDelta = Vector2.SmoothDamp(
            currentLookDelta,
            targetLookDelta,
            ref lookSmoothVel,
            lookSmoothTime
        );

        yaw += currentLookDelta.x;
        pitch -= currentLookDelta.y;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        transform.rotation = Quaternion.Euler(0f, yaw, 0f);

        if (cameraHolder != null)
        {
            cameraHolder.localRotation = Quaternion.Euler(pitch, 0f, 0f);
        }
    }

    void HandleMovement()
    {
        float targetSpeed = isSprinting ? sprintSpeed : moveSpeed;
        Vector3 moveDirection = (transform.forward * moveInput.y + transform.right * moveInput.x).normalized;
        Vector3 targetVelocity = moveDirection * targetSpeed;

        Vector3 currentHorizontalVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        Vector3 smoothedVel = Vector3.MoveTowards(
            currentHorizontalVel,
            targetVelocity,
            acceleration * Time.fixedDeltaTime
        );

        rb.linearVelocity = new Vector3(smoothedVel.x, rb.linearVelocity.y, smoothedVel.z);

        if (jumpRequested)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            jumpRequested = false;
        }
    }

    void CheckGrounded()
    {
        // ponytail: raycast sederhana dari center collider
        Vector3 origin = col != null ? col.bounds.center : transform.position;
        float extraDist = col != null ? (col.bounds.extents.y + groundDistance) : 1f;

        int layer = groundLayer.value == 0 ? ~0 : groundLayer.value;
        isGrounded = Physics.Raycast(origin, Vector3.down, extraDist, layer);
    }
}
