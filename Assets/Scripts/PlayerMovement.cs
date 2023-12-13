using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")] 
    [SerializeField] private float moveSpeed;

    [SerializeField] private float groundDrag;

    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpCooldown;
    [SerializeField] private float airMultiplier;
    private bool readyToJump;

    [Header("Keybinds")] 
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;

    [Header("Ground Check")] 
    [SerializeField] private float playerHeight;
    [SerializeField] private LayerMask ground;
    private bool grounded;
    
    [SerializeField] private Transform orientation;

    private float horizontalInput;
    private float verticalInput;

    private Vector3 moveDirection;

    private Rigidbody myRigidBody;
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody>();
        myRigidBody.freezeRotation = true;
        readyToJump = true;
    }
    
    void Update()
    {
        //ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, ground);
        
        //handle drag
        if (grounded)
        {
            myRigidBody.drag = groundDrag;
        }
        else
        {
            myRigidBody.drag = 0;
        }
        
        MyInput();
        SpeedControl();
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        
        //when to jump
        if (Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;
            Jump();
            
            //continuously jump while holding jump key (expensive!)
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void MovePlayer()
    {
        //calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        
        //on ground
        //10f for smaller numbers in inspector
        if (grounded)
        {
            myRigidBody.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }
        //air
        else if (!grounded)
        {
            myRigidBody.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }
    }

    private void SpeedControl()
    {
        Vector3 flatVelocity = new Vector3(myRigidBody.velocity.x, 0f, myRigidBody.velocity.z);
        
        //limit velocity if max speed exceeded
        if (flatVelocity.magnitude > moveSpeed)
        {
            //what the max velocity should be
            Vector3 limitedVelocity = flatVelocity.normalized * moveSpeed;
            myRigidBody.velocity = new Vector3(limitedVelocity.x, myRigidBody.velocity.y, limitedVelocity.z);
        }
    }

    private void Jump()
    {
        //reset y velocity
        myRigidBody.velocity = new Vector3(myRigidBody.velocity.x, 0f, myRigidBody.velocity.z);
        
        myRigidBody.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }
}
