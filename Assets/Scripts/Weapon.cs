using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private Transform cam;
    [SerializeField] private float range = 100f;
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        RaycastHit hit;
        Physics.Raycast(cam.position, cam.forward, out hit, range);
        Debug.Log("Hit: " + hit.transform.name);
    }
}
