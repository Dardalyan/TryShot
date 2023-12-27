using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackPlayer : MonoBehaviour
{
    private Transform target;

    [SerializeField] private int speed = 10;


    private void Start()
    {
        target = GameObject.Find("player").transform;
    }


    // Update is called once per frame
    void Update()
    {
        Vector3 relativePos = target.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(relativePos);

        Quaternion current = transform.localRotation;

        transform.localRotation = Quaternion.Slerp(current, rotation, Time.deltaTime * speed);
    }


}
