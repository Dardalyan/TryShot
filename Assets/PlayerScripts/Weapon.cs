using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private Transform cam;
    [SerializeField] private float range = 100f;
    [SerializeField] private Ammo ammoSlot;
    [SerializeField] private float timeBetweenShots = 0.5f;
    [SerializeField] private float damage = 25;
    [SerializeField] private AmmoType ammoType;
    [SerializeField] private TextMeshProUGUI ammoText;

    private bool canShoot = true;

    private void OnEnable()
    {
        canShoot = true;
    }

    void Update()
    {
        DisplayAmmo();
        if(Input.GetMouseButtonDown(0) && canShoot == true)
        {
            StartCoroutine(Shoot());
        }
    }

    private void DisplayAmmo()
    {
        int currentAmmo = ammoSlot.GetCurrentAmmo(ammoType);
        ammoText.text = currentAmmo.ToString();
    }

    IEnumerator Shoot()
    {
        canShoot = false;
        
        if (ammoSlot.GetCurrentAmmo(ammoType) > 0)
        {
            ProcessRaycast();
            ammoSlot.ReduceCurrentAmmo(ammoType);
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
