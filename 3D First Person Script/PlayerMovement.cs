// Script Made By Exsr6

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    // === Movement Settings ===
    [Header("Movement Settings")]
    public float walkSpeed = 6f;
    public float sprintSpeed = 9f;
    public float crouchSpeed = 3f;
    public float gravity = -9.81f;

    // === Stamina Settings ===
    [Header("Stamina Settings")]
    public float maxStamina = 5f;
    public float staminaDrain = 1f;
    public float staminaRegen = 1.5f;

    // === Crouch Settings ===
    [Header("Crouch Settings")]
    public float crouchHeight = 1f;
    public float standHeight = 2f;
    public float crouchTransitionSpeed = 6f;

    // === Ground Check ===
    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    // === References ===
    [Header("References")]
    public Transform playerCamera;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;

    private float currentSpeed;
    private float stamina;
    private bool isSprinting;
    private bool isCrouching;

    void Start() {
        controller = GetComponent<CharacterController>();
        stamina = maxStamina;
    }

    void Update() {
        // Ground Check
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        // Input
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = transform.right * x + transform.forward * z;

        HandleCrouch();
        HandleSprint(z);

        controller.Move(move * currentSpeed * Time.deltaTime);

        // Gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void HandleSprint(float forwardInput) {
        bool wantsToSprint = Input.GetKey(KeyCode.LeftShift) && forwardInput > 0 && !isCrouching;
        if (wantsToSprint && stamina > 0f) {
            isSprinting = true;
            currentSpeed = sprintSpeed;
            stamina -= staminaDrain * Time.deltaTime;
        }
        else {
            isSprinting = false;
            currentSpeed = isCrouching ? crouchSpeed : walkSpeed;

            if (stamina < maxStamina)
                stamina += staminaRegen * Time.deltaTime;
        }

        stamina = Mathf.Clamp(stamina, 0f, maxStamina);
    }

    void HandleCrouch() {
        if (Input.GetKeyDown(KeyCode.LeftControl)) {
            isCrouching = !isCrouching;
        }

        float targetHeight = isCrouching ? crouchHeight : standHeight;
        controller.height = Mathf.Lerp(controller.height, targetHeight, Time.deltaTime * crouchTransitionSpeed);

        Vector3 camPos = playerCamera.localPosition;
        camPos.y = Mathf.Lerp(camPos.y, isCrouching ? 0.5f : 1f, Time.deltaTime * crouchTransitionSpeed);
        playerCamera.localPosition = camPos;
    }

    public bool IsMoving() {
        return controller.velocity.magnitude > 0.1f;
    }

    public bool IsGrounded() {
        return isGrounded;
    }

    public bool IsSprinting() {
        return isSprinting;
    }

    public bool IsCrouching() {
        return isCrouching;
    }
}
