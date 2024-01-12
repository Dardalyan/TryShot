using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponZoom : MonoBehaviour
{
    [SerializeField] private PlayerCam playerCam;

    [SerializeField] private float zoomedInFOV = 20f;

    [SerializeField] private float zoomedOutSensitivity = 1000;
    [SerializeField] private float zoomedInSensitivity = 500f;

    private bool zoomedInToggle = false;
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (zoomedInToggle == false)
            {
                zoomedInToggle = true;
                playerCam.DoFov(zoomedInFOV);
                playerCam.sensitivityX = zoomedInSensitivity;
                playerCam.sensitivityY = zoomedInSensitivity;
            }
            else
            {
                zoomedInToggle = false;
                playerCam.DoFov(85);
                playerCam.sensitivityX = zoomedOutSensitivity;
                playerCam.sensitivityY = zoomedOutSensitivity;
            }
        }
    }
}
