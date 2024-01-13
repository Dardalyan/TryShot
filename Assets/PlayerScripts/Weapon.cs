using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private Transform cam;
    [SerializeField] private float range = 100f;
    [SerializeField] private Ammo ammoSlot;
    //damage variable
    
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        if (ammoSlot.GetCurrentAmmo() > 0)
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
        
            ammoSlot.ReduceCurrentAmmo();
        }
      
    }
}
