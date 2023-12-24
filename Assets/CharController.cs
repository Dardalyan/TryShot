using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class CharController : MonoBehaviour
{
    
    private CharacterController controller;
    private float Speed = 25f;
    private Vector3 moveVector;
    
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        moveCharacter();
    }

    private void moveCharacter()
    {
        Vector3 move = new Vector3(Input.GetAxis("Horizontal")*-1, 0,Input.GetAxis("Vertical")*-1);
        move = transform.TransformDirection(move);
        controller.Move(move * (Time.deltaTime * Speed));    
    }
}
