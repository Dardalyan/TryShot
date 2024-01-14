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
    
    [Header("AimPrediction")] 
    private RaycastHit predictionHit;
    [SerializeField] private float predictionSphereCastRadius;
    [SerializeField] private Transform predictionPoint;
    
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
        
        CheckForGrapplePoints();

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
        if (grapplingCoolDownTimer > 0 || predictionHit.point == Vector3.zero)
        {
            return;
        }
        
        predictionPoint.gameObject.SetActive(false);
        
        //deactivate active swinging
        GetComponent<Swinging>().StopSwing();

        isGrappling = true;

        playerMovementScript.isFrozen = true;
        
        grapplePoint = predictionHit.point;
            
        //nameof gets the methods name as a string
        Invoke(nameof(ExecuteGrapple), grappleDelayTime);
        
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
    
    private void CheckForGrapplePoints()
    {
        //if currentşy swinging, return
        if (isGrappling || playerMovementScript.isSwinging)
        {
            return;
        }

        RaycastHit sphereCastHit;
        Physics.SphereCast(cam.position, predictionSphereCastRadius, cam.forward, out sphereCastHit, maxGrappleDistance, grappleable);

        RaycastHit raycastHit;
        Physics.Raycast(cam.position, cam.forward, out raycastHit, maxGrappleDistance, grappleable);

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
