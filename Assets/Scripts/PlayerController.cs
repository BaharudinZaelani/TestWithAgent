using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float acceleration = 30f;
    public float jumpForce = 5f;
    public Transform cameraTransform;

    [Header("Ground Check")]
    public float groundDistance = 0.2f;
    public LayerMask groundLayer;

    private Rigidbody rb;
    private Collider col;
    private Vector3 inputDir;
    private bool jumpRequested;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        rb.freezeRotation = true;
        // ponytail: interpolation smooth render frame; matikan jika pakai custom kinematic solver
        rb.interpolation = RigidbodyInterpolation.Interpolate;

        if (cameraTransform == null && Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }
    }

    void Update()
    {
        if (Keyboard.current == null) return;

        float moveX = 0f;
        float moveZ = 0f;

        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) moveX -= 1f;
        if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) moveX += 1f;
        if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) moveZ -= 1f;
        if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) moveZ += 1f;

        inputDir = new Vector3(moveX, 0f, moveZ).normalized;

        if (Keyboard.current.spaceKey.wasPressedThisFrame && isGrounded)
        {
            jumpRequested = true;
        }
    }

    void FixedUpdate()
    {
        CheckGrounded();

        Vector3 moveDirection = inputDir;
        if (cameraTransform != null)
        {
            Vector3 camForward = cameraTransform.forward;
            Vector3 camRight = cameraTransform.right;
            camForward.y = 0f;
            camRight.y = 0f;
            camForward.Normalize();
            camRight.Normalize();

            moveDirection = (camRight * inputDir.x + camForward * inputDir.z).normalized;
        }

        Vector3 targetVelocity = moveDirection * moveSpeed;
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
        // ponytail: raycast sederhana dari center collider; ganti spherecast bila karakter naik turun lereng
        Vector3 origin = col != null ? col.bounds.center : transform.position;
        float extraDist = col != null ? (col.bounds.extents.y + groundDistance) : 1f;

        int layer = groundLayer.value == 0 ? ~0 : groundLayer.value;
        isGrounded = Physics.Raycast(origin, Vector3.down, extraDist, layer);
    }
}
