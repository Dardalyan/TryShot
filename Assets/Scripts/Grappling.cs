using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Grappling : MonoBehaviour
{
    [Header("References")] 
    private PlayerMovement playerMovementScript;
    [SerializeField] private Transform cam;
    [SerializeField] private Transform gunTip;
    [SerializeField] private LayerMask grappleable;
    [SerializeField] private LineRenderer lineRenderer;

    [Header("Grappling")] 
    [SerializeField] private float maxGrappleDistance;
    [SerializeField] private float grappleDelayTime;
    [SerializeField] private float overShootYAxis;

    private Vector3 grapplePoint;

    [Header("Cooldown")] 
    [SerializeField] private float grapplingCoolDown;
    private float grapplingCoolDownTimer;

    [Header("Input")] 
    [SerializeField] private KeyCode grappleKey = KeyCode.Mouse1;
    
    private bool isGrappling;

    private void Start()
    {
        playerMovementScript = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(grappleKey))
        {
            StartGrapple();
        }

        if (grapplingCoolDownTimer > 0)
        {
            grapplingCoolDownTimer -= Time.deltaTime;
        }
    }

    private void LateUpdate()
    {
        if (isGrappling)
        {
            lineRenderer.SetPosition(0, gunTip.position);
        }
    }

    private void StartGrapple()
    {
        if (grapplingCoolDownTimer > 0)
        {
            return;
        }

        isGrappling = true;

        playerMovementScript.isFrozen = true;

        RaycastHit raycastHit;
        if (Physics.Raycast(cam.position, cam.forward, out raycastHit, maxGrappleDistance, grappleable))
        {
            grapplePoint = raycastHit.point;
            
            //nameof gets the methods name as a string
            Invoke(nameof(ExecuteGrapple), grappleDelayTime);
        }
        else
        {
            grapplePoint = cam.position + cam.forward * maxGrappleDistance;
            
            Invoke(nameof(StopGrapple), grappleDelayTime);
        }

        lineRenderer.enabled = true;
        lineRenderer.SetPosition(1, grapplePoint);

    }

    private void ExecuteGrapple()
    {
        playerMovementScript.isFrozen = false;

        Vector3 lowestPoint = new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z);

        float grapplePointRelativeYPosition = grapplePoint.y - lowestPoint.y;
        float highestPointOnArc = grapplePointRelativeYPosition + overShootYAxis;

        if (grapplePointRelativeYPosition < 0)
        {
            highestPointOnArc = overShootYAxis;
        }
        
        playerMovementScript.JumpToPosition(grapplePoint, highestPointOnArc);
        
        Invoke(nameof(StopGrapple), 1f);
    }

    public void StopGrapple()
    {
        playerMovementScript.isFrozen = false;
        
        isGrappling = false;

        grapplingCoolDownTimer = grapplingCoolDown;

        lineRenderer.enabled = false;
    }
}
