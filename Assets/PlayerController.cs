using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 5f;

    private Rigidbody rb;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // Mencegah rotasi akibat benturan fisik
        rb.freezeRotation = true;
    }

    void Update()
    {
        float moveX = Input.GetAxis("Horizontal"); // A/D atau Left/Right Arrow
        float moveZ = Input.GetAxis("Vertical");   // W/S atau Up/Down Arrow

        Vector3 moveDir = new Vector3(moveX, 0f, moveZ).normalized;
        Vector3 velocity = moveDir * moveSpeed;
        velocity.y = rb.velocity.y; // Pertahankan gravitasi vertikal

        rb.velocity = velocity;

        // Lompat sederhana dengan spasi
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    void OnCollisionStay(Collision collision)
    {
        // Deteksi sederhana apakah menyentuh lantai
        if (collision.gameObject.name == "Ground")
        {
            isGrounded = true;
        }
    }
}
