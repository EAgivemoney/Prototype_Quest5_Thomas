using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] float baseSpeed = 1f;
    [SerializeField] float boostModifier = 1f;
    [SerializeField] float jumpHeight = 1f;
    [SerializeField] float gravity = -30f;
    [SerializeField] int numberOfJumps = 2;

    [Header("Ground Check Settings")]
    [SerializeField] Transform groundCheckPoint;
    [SerializeField] float groundCheckRadius = 0.3f;
    [SerializeField] LayerMask groundLayers;

    CharacterController characterController;
    Vector3 velocity;

    bool isGrounded;
    int jumpCounter;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        CheckIfGrounded();
        HandleJump();
        HandleHorizontalMovement();

        characterController.Move(velocity * Time.deltaTime);
    }

    void CheckIfGrounded()
    {
        isGrounded = Physics.CheckSphere(groundCheckPoint.position, groundCheckRadius, groundLayers, QueryTriggerInteraction.Ignore);

        if (isGrounded && velocity.y < 0f)
        {
            velocity.y = -10f;
            jumpCounter = numberOfJumps; // Reset jump counter when grounded and falling
        }

        velocity.y += gravity * Time.deltaTime;
    }

    void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && jumpCounter > 0)
        {
            float jumpPower = (jumpCounter == numberOfJumps) ? jumpHeight : jumpHeight * 0.5f;
            velocity.y = Mathf.Sqrt(jumpPower * -2f * gravity);
            jumpCounter--;
        }
    }

    void HandleHorizontalMovement()
    {
        float horizontalSpeed = Input.GetKey(KeyCode.D) ? baseSpeed + boostModifier : baseSpeed;
        characterController.Move(new Vector3(horizontalSpeed, 0f, 0f) * Time.deltaTime);
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheckPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheckPoint.position, groundCheckRadius);
        }
    }
}