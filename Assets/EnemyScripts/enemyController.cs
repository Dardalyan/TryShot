using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class enemyController : MonoBehaviour
{
    private Ray ray;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var iot = isOnTarget();
        Debug.Log(iot);
        
       
    }

    public bool isOnTarget()
    {
        ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        bool state = false;

        if (Physics.Raycast(ray, out  hit))
        {
            if (hit.collider.gameObject.name == "user")
            {
                state = true;
            }
        }

        return state;

    }
    
    
}
