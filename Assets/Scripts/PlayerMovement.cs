using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")] 
    private float moveSpeed;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float sprintSpeed;
    [SerializeField] private float slideSpeed;
    [SerializeField] private float wallRunSpeed;
    
    private float desiredMoveSpeed;
    private float lastDesiredMoveSpeed;

    [SerializeField] private float speedIncreaseMultiplier;
    [SerializeField] private float slopeIncreaseMultiplier;
    
    
    [SerializeField] private float groundDrag;

    [Header("Jumping")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpCooldown;
    [SerializeField] private float airMultiplier;
    private bool readyToJump;

    [Header("Crouching")] 
    [SerializeField] private float crouchSpeed;
    [SerializeField] private float crouchYScale;
    private float startYScale;

    [Header("Keybinds")] 
    [SerializeField] private KeyCode jumpKey = KeyCode.Space; 
    [SerializeField] private KeyCode sprintKey = KeyCode.LeftShift;
    [SerializeField] private KeyCode crouchKey = KeyCode.LeftControl;
    
    [Header("Ground Check")] 
    [SerializeField] private float playerHeight;
    [SerializeField] private LayerMask ground;
    private bool grounded;

    [Header("Slope Check")] 
    [SerializeField] private float maxSlopeAngle;
    private RaycastHit slopeRaycastHit;
    private bool exitingSlope;
    
    
    [SerializeField] private Transform orientation;

    private float horizontalInput;
    private float verticalInput;

    private Vector3 moveDirection;

    private Rigidbody myRigidBody;

    [SerializeField] private MovementState movementState;

    public enum MovementState
    {
        walking,
        sprinting,
        crouching,
        sliding,
        air,
        wallrunning
    }

    public bool isSliding;
    public bool wallrunning;
    
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody>();
        myRigidBody.freezeRotation = true;
        
        readyToJump = true;

        startYScale = transform.localScale.y;
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
        StateHandler();
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
        
        //crouch
        if (Input.GetKeyDown(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            //to not float in the air after crouching
            myRigidBody.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        }
        
        //stop crouching
        if (Input.GetKeyUp(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
        }
    }

    private void StateHandler()
    {
        //wallrunning
        if (wallrunning)
        {
            movementState = MovementState.wallrunning;
            desiredMoveSpeed = wallRunSpeed;
        }
        
        //sliding
        if (isSliding)
        {
            movementState = MovementState.sliding;

            if (OnSlope() && myRigidBody.velocity.y < 0.1f)
            {
                desiredMoveSpeed = slideSpeed;
            }
            else
            {
                desiredMoveSpeed = sprintSpeed;
            }
        }
        //crouching
        if (Input.GetKey(crouchKey))
        {
            movementState = MovementState.crouching;
            desiredMoveSpeed = crouchSpeed;
        }
        //sprinting
        else if (grounded && Input.GetKey(sprintKey))
        {
            movementState = MovementState.sprinting;
            desiredMoveSpeed = sprintSpeed;
        }
        
        //walking
        else if (grounded)
        {
            movementState = MovementState.walking;
            desiredMoveSpeed = walkSpeed;
        }

        //air
        else
        {
            movementState = MovementState.air;
        }
        
        //check if desiredMoveSpeed has changed drastically. if the move speed changed more than 4, don't change it instantly but lerp it slowly, thus keeping the momentum
        if (Mathf.Abs(desiredMoveSpeed - lastDesiredMoveSpeed) > 8f && moveSpeed != 0)
        {
            StopAllCoroutines();
            StartCoroutine(SmoothlyLerpMoveSpeed());
        }
        else
        {
            moveSpeed = desiredMoveSpeed;
        }

        lastDesiredMoveSpeed = desiredMoveSpeed;
    }

    //coroutine
    private IEnumerator SmoothlyLerpMoveSpeed()
    {
        //smoothly lerp moveSpeed to desired value
        float time = 0;
        float difference = Mathf.Abs(desiredMoveSpeed - moveSpeed);
        float startValue = moveSpeed;

        while (time < difference)
        {
            moveSpeed = Mathf.Lerp(startValue, desiredMoveSpeed, time / difference);

            if (OnSlope())
            {
                //steeper slope = more acceleration
                float slopeAngle = Vector3.Angle(Vector3.up, slopeRaycastHit.normal);
                float slopeAngleIncrease = 1 + (slopeAngle / 90f);
                
                time += Time.deltaTime * speedIncreaseMultiplier * slopeIncreaseMultiplier * slopeAngleIncrease;
            }
            else
            {
                time += Time.deltaTime * speedIncreaseMultiplier;
            }
            
            yield return null;
        }

        moveSpeed = desiredMoveSpeed;
    }
    
    private void MovePlayer()
    {
        //calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        
        //on slope
        if (OnSlope() && !exitingSlope)
        {
            myRigidBody.AddForce(GetSlopeMoveDirection(moveDirection) * (moveSpeed * 20f), ForceMode.Force);
            
            //to stop the player from bumping on the slope and jumping, add a downward force and keep the player on the slope
            if (myRigidBody.velocity.y > 0)
            {
                myRigidBody.AddForce(Vector3.down * 80f, ForceMode.Force);
            }
        }
        
        //on ground
        //10f for smaller numbers in inspector
        else if (grounded)
        {
            myRigidBody.AddForce(moveDirection.normalized * (moveSpeed * 10f), ForceMode.Force);
        }
        //in air
        else if (!grounded)
        {
            myRigidBody.AddForce(moveDirection.normalized * (moveSpeed * 10f * airMultiplier), ForceMode.Force);
        }
        
        //turn gravity off while on slope
        myRigidBody.useGravity = !OnSlope();
    }

    private void SpeedControl()
    {
        //limiting speed on slope
        if (OnSlope() && !exitingSlope)
        {
            if (myRigidBody.velocity.magnitude > moveSpeed)
            {
                myRigidBody.velocity = myRigidBody.velocity.normalized * moveSpeed;
            }
        }

        //limiting speed on ground or in air
        else
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
    }

    private void Jump()
    {
        exitingSlope = true;
        
        //reset y velocity
        myRigidBody.velocity = new Vector3(myRigidBody.velocity.x, 0f, myRigidBody.velocity.z);
        
        myRigidBody.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;

        exitingSlope = false;
    }

    public bool OnSlope()
    {   //"out" stores the information of the object hit
        if (Physics.Raycast(transform.position, Vector3.down, out slopeRaycastHit, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeRaycastHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }

    public Vector3 GetSlopeMoveDirection(Vector3 direction)
    {
        return Vector3.ProjectOnPlane(direction, slopeRaycastHit.normal).normalized;
    }
}
