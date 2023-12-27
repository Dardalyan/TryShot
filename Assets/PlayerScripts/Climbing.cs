using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Climbing : MonoBehaviour
{
    [Header("References")] 
    [SerializeField] private Transform orientation;
    [SerializeField] private Rigidbody myRigidbody;
    [SerializeField] private LayerMask wall;
    [SerializeField] private PlayerMovement playerMovement;

    [Header("Climbing")] 
    [SerializeField] private float climbSpeed;
    [SerializeField] private float maxClimbTime;
    private float climbTimer;

    private bool isClimbing;

    [Header("ClimbJumping")] 
    [SerializeField] private float climbJumpUpForce;
    [SerializeField] private float climbJumpBackForce;

    [SerializeField] private KeyCode jumpKey = KeyCode.Space;
    [SerializeField] private int climbJumps;
    private int climbJumpsLeft;
    
    [Header("Detection")] 
    [SerializeField] private float detectionLength;
    [SerializeField] private float sphereCastRadius;
    [SerializeField] private float maxWallLookAngle;
    private float wallLookAngle;

    private RaycastHit frontWallRaycastHit;
    private bool wallFrontExists;

    private Transform lastWall;
    private Vector3 lastWallNormal;
    [SerializeField] private float minWallNormalAngleChange;

    [Header("Exiting")] 
    public bool exitingWall;
    [SerializeField] private float exitWallTime;
    private float exitWallTimer;
    
    private void Update()
    {
        WallCheck();
        StateMachine();

        //not in fixedupdate because we do not add force, we change the velocity
        if (isClimbing && !exitingWall)
        {
            ClimbingMovement();
        }
    }

    private void StateMachine()
    {
        //climbing state
        if (wallFrontExists && Input.GetKey(KeyCode.W) && wallLookAngle < maxWallLookAngle && !exitingWall)
        {
            if (!isClimbing && climbTimer > 0)
            {
                StartClimbing();
            }
            
            //timer
            if (climbTimer > 0)
            {
                climbTimer -= Time.deltaTime;
            }

            if (climbTimer < 0)
            {
                StopClimbing();
            }
        }
        
        else if (exitingWall)
        {
            if (isClimbing)
            {
                StopClimbing();
            }

            if (exitWallTimer > 0)
            {
                exitWallTimer -= Time.deltaTime;
            }

            if (exitWallTimer < 0)
            {
                exitingWall = false;
            }
            
        }
        
        else
        {
            if (isClimbing)
            {
                StopClimbing();
            }
        }

        if (wallFrontExists && Input.GetKeyDown(jumpKey) && climbJumpsLeft > 0)
        {
            ClimbJump();
        }
    }

    private void WallCheck()
    {
        wallFrontExists = Physics.SphereCast(transform.position, sphereCastRadius, orientation.forward, out frontWallRaycastHit, detectionLength, wall);
        wallLookAngle = Vector3.Angle(orientation.forward, -frontWallRaycastHit.normal);

        bool newWall = frontWallRaycastHit.transform != lastWall || Mathf.Abs(Vector3.Angle(lastWallNormal, frontWallRaycastHit.normal)) > minWallNormalAngleChange;
        
        if ((wallFrontExists && newWall) || playerMovement.grounded)
        {
            climbTimer = maxClimbTime;
            climbJumpsLeft = climbJumps;
        }
    }

    private void StartClimbing()
    {
        isClimbing = true;
        playerMovement.isClimbing = true;

        lastWall = frontWallRaycastHit.transform;
        lastWallNormal = frontWallRaycastHit.normal;

        //camera fov change may be implemented
    }

    private void ClimbingMovement()
    {
        myRigidbody.velocity = new Vector3(myRigidbody.velocity.x, climbSpeed, myRigidbody.velocity.z);
    }

    private void StopClimbing()
    {
        isClimbing = false;
        playerMovement.isClimbing = false;
    }

    private void ClimbJump()
    {
        exitingWall = true;
        exitWallTimer = exitWallTime;
        
        Vector3 forceToApply = transform.up * climbJumpUpForce + frontWallRaycastHit.normal * climbJumpBackForce;

        myRigidbody.velocity = new Vector3(myRigidbody.velocity.x, 0f, myRigidbody.velocity.z);
        myRigidbody.AddForce(forceToApply, ForceMode.Impulse);

        climbJumpsLeft--;
    }
}
