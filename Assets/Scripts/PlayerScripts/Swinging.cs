using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Swinging : MonoBehaviour
{
    [Header("Input")] 
    [SerializeField] private KeyCode swingKey = KeyCode.Mouse0;

    [Header("References")] 
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Transform gunTip, cam, player;
    [SerializeField] private LayerMask grappleable;
    [SerializeField] private PlayerMovement playerMovementScript;

    [Header("Swinging")] 
    [SerializeField] private float maxSwingDistance = 25f;
    private Vector3 swingPoint;
    private SpringJoint joint;

    [Header("AirMovement")] 
    [SerializeField] private Transform orientation;
    [SerializeField] private Rigidbody myRigidBody;
    [SerializeField] private float horizontalAirForce;
    [SerializeField] private float forwardAirForce;
    [SerializeField] private float extendCableSpeed;

    [Header("AimPrediction")] 
    private RaycastHit predictionHit;
    [SerializeField] private float predictionSphereCastRadius;
    [SerializeField] private Transform predictionPoint;
    
    private void Update()
    {
        if (Input.GetKeyDown(swingKey))
        {
            StartSwing();
        }

        if (Input.GetKeyUp(swingKey))
        {
            StopSwing();
        }
        
        CheckForSwingPoints();

        if (joint != null)
        {
            AirMovementWhileSwinging();
        }
    }

    private void LateUpdate()
    {
        DrawRope();
    }

    private void StartSwing()
    {
        //return if predictionHit not found
        if (predictionHit.point == Vector3.zero)
        {
            return;
        }
        
        predictionPoint.gameObject.SetActive(false);
        
        //deactivate active grapple
        if (GetComponent<Grappling>() != null)
        {
            GetComponent<Grappling>().StopGrapple();
        }
        playerMovementScript.ResetRestrictions();
        
        playerMovementScript.isSwinging = true;

        swingPoint = predictionHit.point;
        joint = player.gameObject.AddComponent<SpringJoint>();
        joint.autoConfigureConnectedAnchor = false;
        joint.connectedAnchor = swingPoint;

        float distanceFrompoint = Vector3.Distance(player.position, swingPoint);
            
        //the distance grapple will try to keep from grapple point
        joint.maxDistance = distanceFrompoint * 0.8f;
        joint.minDistance = distanceFrompoint * 0.25f;

        joint.spring = 4.5f;
        joint.damper = 7f;
        joint.massScale = 4.5f;

        lineRenderer.positionCount = 2;
        currentGrapplePosition = gunTip.position;
        
    }

    private Vector3 currentGrapplePosition;

    private void DrawRope()
    {
        //if not grappling don't draw the rope
        if (!joint)
        {
            return;
        }

        currentGrapplePosition = Vector3.Lerp(currentGrapplePosition, swingPoint, Time.deltaTime * 8f);
        
        lineRenderer.SetPosition(0, gunTip.position);
        lineRenderer.SetPosition(1, swingPoint);
    }

    public void StopSwing()
    {
        playerMovementScript.isSwinging = false;
        
        lineRenderer.positionCount = 0;
        Destroy(joint);
    }

    private void AirMovementWhileSwinging()
    {
        //right
        if (Input.GetKey(KeyCode.D))
        {
            myRigidBody.AddForce(orientation.right * horizontalAirForce * Time.deltaTime);
        }
        //left
        if (Input.GetKey(KeyCode.A))
        {
            myRigidBody.AddForce(-orientation.right * horizontalAirForce * Time.deltaTime);
        }
        //forward
        if (Input.GetKey(KeyCode.W))
        {
            myRigidBody.AddForce(orientation.forward * forwardAirForce * Time.deltaTime);
        }
        
        //shorten cable
        if (Input.GetKey(KeyCode.Space))
        {
            Vector3 directionToPoint = swingPoint - transform.position;
            myRigidBody.AddForce(directionToPoint.normalized * forwardAirForce * Time.deltaTime);

            float distanceFromPoint = Vector3.Distance(transform.position, swingPoint);

            joint.maxDistance = distanceFromPoint * 0.8f;
            joint.minDistance = distanceFromPoint * 0.25f;
        }
        //extend cable
        if (Input.GetKey(KeyCode.S))
        {
            float extendedDistanceFromPoint = Vector3.Distance(transform.position, swingPoint) + extendCableSpeed;

            joint.maxDistance = extendedDistanceFromPoint * 0.8f;
            joint.minDistance = extendedDistanceFromPoint * 0.25f;
        }
    }

    private void CheckForSwingPoints()
    {
        //if currentşy swinging, return
        if (joint != null)
        {
            return;
        }

        RaycastHit sphereCastHit;
        Physics.SphereCast(cam.position, predictionSphereCastRadius, cam.forward, out sphereCastHit, maxSwingDistance, grappleable);

        RaycastHit raycastHit;
        Physics.Raycast(cam.position, cam.forward, out raycastHit, maxSwingDistance, grappleable);

        Vector3 realHitPoint;
        //option 1 - direct hit
        if (raycastHit.point != Vector3.zero)
        {
            realHitPoint = raycastHit.point;
        }
        
        //option 2 - predicted hit
        else if (sphereCastHit.point != Vector3.zero)
        {
            realHitPoint = sphereCastHit.point;
        }

        //option 3 - miss
        else
        {
            realHitPoint = Vector3.zero;
        }
        
        //realhitpoint is not zero
        if (realHitPoint != Vector3.zero)
        {
            predictionPoint.gameObject.SetActive(true);
            predictionPoint.position = realHitPoint;
        }
        //realhitpoint is zero
        else
        {
            predictionPoint.gameObject.SetActive(false);
        }

        predictionHit = raycastHit.point == Vector3.zero ? sphereCastHit : raycastHit;
    }
}
