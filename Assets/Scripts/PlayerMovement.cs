using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] LayerMask groundLayers;
    [SerializeField] float gravity = -30f;
    [SerializeField] float baseSpeed = 1f;
    [SerializeField] float jumpHeight = 1f;
    [SerializeField] float distanceCheck = 1f;
    [SerializeField] float boostModifier = 1f;
    [SerializeField] int numberOfJumps = 2;
    [SerializeField] float horizontalSpeed;

    CharacterController characterController;
    Vector3 velocity;

    bool isGrounded;
    bool isBoosting;
    int jumpCounter;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        CheckIfGrounded();
        RunForrestRun();
        ProcessJump();

        characterController.Move(velocity * Time.deltaTime);
    }

    void ProcessJump()
    {       
        if (isGrounded)
        {
            jumpCounter = numberOfJumps;
        }

        if (jumpCounter < 1) { return; }

        if (Input.GetButtonDown("Jump"))
        {
            velocity.y += Mathf.Sqrt(jumpHeight * -2 * gravity);
            jumpCounter --;
        }
    }

    void RunForrestRun()
    {
        if (Input.GetKey("d"))
        {
            horizontalSpeed = baseSpeed + boostModifier;
        }
        else
        {
            horizontalSpeed = baseSpeed;
        }
        characterController.Move(new Vector3(horizontalSpeed, 0f, 0f) * Time.deltaTime);

    }

    void CheckIfGrounded()
    {
        isGrounded = Physics.CheckSphere(transform.position, distanceCheck, groundLayers, QueryTriggerInteraction.Ignore);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = 0;
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
        }
    }
}
