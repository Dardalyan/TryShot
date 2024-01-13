using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private Transform cam;
    [SerializeField] private float range = 100f;
    [SerializeField] private Ammo ammoSlot;
    [SerializeField] private float timeBetweenShots = 0.5f;
    [SerializeField] private float damage = 25;

    private bool canShoot = true;
    
    void Update()
    {
        if(Input.GetMouseButtonDown(0) && canShoot == true)
        {
            StartCoroutine(Shoot());
        }
    }

    IEnumerator Shoot()
    {
        canShoot = false;
        
        if (ammoSlot.GetCurrentAmmo() > 0)
        {
            ProcessRaycast();
            ammoSlot.ReduceCurrentAmmo();
        }

        yield return new WaitForSeconds(timeBetweenShots);
        
        canShoot = true;
    }

    private void ProcessRaycast()
    {
        RaycastHit hit;
        if(Physics.Raycast(cam.position, cam.forward, out hit, range))
        {
            Debug.Log("Hit: " + hit.transform.name);
            //add shooting effect
            //set a target (getcomponent enemy health)
            //if (target == null){return;}
            //call enemy healt decrease method
        }
        else
        {
            return;
        }
    }
}
