using UnityEngine;
using UnityEngine.InputSystem;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float distance = 6f;
    public float height = 2f;
    public float mouseSensitivity = 0.2f;
    public float minPitch = -20f;
    public float maxPitch = 60f;

    private float yaw;
    private float pitch = 20f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Vector3 angles = transform.eulerAngles;
        yaw = angles.y;
        pitch = angles.x;
    }

    void LateUpdate()
    {
        if (target == null) return;

        if (Mouse.current != null)
        {
            Vector2 delta = Mouse.current.delta.ReadValue() * mouseSensitivity;
            yaw += delta.x;
            pitch -= delta.y;
            pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
        }

        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);
        Vector3 targetFocus = target.position + Vector3.up * height;
        // ponytail: orbit jarak tetap; tambah raycast camera collision jika kamera tembus dinding
        Vector3 desiredPos = targetFocus - (rotation * Vector3.forward * distance);

        transform.position = desiredPos;
        transform.LookAt(targetFocus);
    }
}

