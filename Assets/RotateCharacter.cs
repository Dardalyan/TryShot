using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCharacter : MonoBehaviour
{
    private float rotateY = 0;
    private float rotateX = 0;
    private float rotateZ = 0;
    private float sensivity = 3;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RotateBody();
    }

    private void RotateBody()
    {
        rotateY += sensivity * Input.GetAxis("Mouse Y") ;
        rotateX += sensivity * Input.GetAxis("Mouse X");
        transform.eulerAngles = new Vector3(rotateY, rotateX, rotateZ);
    }
}
