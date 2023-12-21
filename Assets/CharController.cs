using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class CharController : MonoBehaviour
{
    
    private CharacterController controller;
    private float Speed = 25f;
    
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        
        
        
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0,Input.GetAxis("Vertical"));
        controller.Move(move * (Time.deltaTime * Speed));

    }
}
