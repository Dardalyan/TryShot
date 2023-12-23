using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRunning : MonoBehaviour
{
    [Header("Wallrunning")] 
    [SerializeField] private LayerMask wall;
    [SerializeField] private LayerMask ground;
    [SerializeField] private float wallRunForce;
    [SerializeField] private float wallClimbSpeed;
    [SerializeField] private float maxWallRunTime;
    private float wallRunTimer;

    [Header("Input")] 
    [SerializeField] private KeyCode upwardsRunKey = KeyCode.LeftShift;
    [SerializeField] private KeyCode downwardsRunKey = KeyCode.LeftControl;
    private bool isUpwardsRunning;
    private bool isDownwardsRunning;
    private float horizontalInput;
    private float verticalInput;

    [Header("Detection")] 
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private float minJumpHeight;
    private RaycastHit leftWallRaycastHit;
    private RaycastHit rightWallRaycastHit;
    private bool wallLeftExists;
    private bool wallRightExists;

    [Header("References")] 
    [SerializeField] private Transform orientation;
    private PlayerMovement playerMovement;
    private Rigidbody myRigidbody;

    private void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        CheckForWall();
        StateMachine();
    }

    private void FixedUpdate()
    {
        if (playerMovement.wallrunning)
        {
            WallRunningMovement();
        }
    }

    private void CheckForWall()
    {
        wallRightExists = Physics.Raycast(transform.position, orientation.right, out rightWallRaycastHit, wallCheckDistance, wall);
        wallLeftExists = Physics.Raycast(transform.position, -orientation.right, out leftWallRaycastHit, wallCheckDistance, wall);
    }

    private bool IsAboveGround()
    {
        return !Physics.Raycast(transform.position, Vector3.down, minJumpHeight, ground);
    }

    private void StateMachine()
    {
        //getting inputs
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        isUpwardsRunning = Input.GetKey(upwardsRunKey);
        isDownwardsRunning = Input.GetKey(downwardsRunKey);
        
        //wallrunning
        if ((wallLeftExists || wallRightExists) && verticalInput > 0 && IsAboveGround())
        {
            if (!playerMovement.wallrunning)
            {
                StartWallRun();
            }
        }
        else
        {
            if (playerMovement.wallrunning)
            {
                StopWallRun();
            }
        }
    }

    private void StartWallRun()
    {
        playerMovement.wallrunning = true;
    }

    private void WallRunningMovement()
    {
        myRigidbody.useGravity = false;
        myRigidbody.velocity = new Vector3(myRigidbody.velocity.x, 0f, myRigidbody.velocity.z);
        
        //if there is a wall to the right, take its normal, otherwise take the left walls normal
        Vector3 wallNormal = wallRightExists ? rightWallRaycastHit.normal : leftWallRaycastHit.normal;

        //cross product of the wall normal and upwards direction
        Vector3 wallForward = Vector3.Cross(wallNormal, transform.up);

        //to find out which direction is closer to where the player is facing. Otherwise we will wallrun backwards sometimes
        if ((orientation.forward - wallForward).magnitude > (orientation.forward - -wallForward).magnitude)
        {
            wallForward = -wallForward;
        }
        
        //forward force
        myRigidbody.AddForce(wallForward * wallRunForce, ForceMode.Force);
        
        //upwards/downwards force
        if (isUpwardsRunning)
        {
            myRigidbody.velocity = new Vector3(myRigidbody.velocity.x, wallClimbSpeed, myRigidbody.velocity.z);
        }

        if (isDownwardsRunning)
        {
            myRigidbody.velocity = new Vector3(myRigidbody.velocity.x, -wallClimbSpeed, myRigidbody.velocity.z);
        }
        
        //force to push player to the wall if the player is not trying to get off of the wall
        if (!(wallLeftExists && horizontalInput > 0) && !(wallRightExists && horizontalInput < 0))
        {
            myRigidbody.AddForce(-wallNormal * 100, ForceMode.Force);
        }
        
    }

    private void StopWallRun()
    {
        playerMovement.wallrunning = false;
    }
}
