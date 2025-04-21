// Script Made By Exsr6

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    // === Look Settings ===
    [Header("Mouse Settings")]
    public float mouseSensitivity = 100f;

    // === References ===
    [Header("References")]
    public Transform playerBody;

    private float xRotation = 0f;

    void Start() {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update() {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Vertical camera rotation (looking up/down)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Horizontal player rotation (turning left/right)
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
