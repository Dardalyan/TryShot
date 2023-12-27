using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class enemyController : MonoBehaviour
{
    private Ray ray;
    private NavMeshAgent _agent;
    private float _health;
    
    
    // Start is called before the first frame update
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _health = 200f;
    }

    // Update is called once per frame
    void Update()
    {
        var iot = isOnTarget();
        if(iot)
            Debug.Log("On The TARGET !!!");
        die();
    }


    private void die()
    {
        if (_health <= 0)
        {
            Destroy(gameObject);
        }
    }
    public bool fire()
    {
        if (_agent.isStopped)
        {
            return true;
        }
        return false;
    }

    public bool hitConfirmed()
    {
        if (fire() && isOnTarget())
        {
            return true;
        }

        return false;
    }

    public float enemyFireDamage()
    {
        if (hitConfirmed())
        {
            return 7.5f;
        }

        return 0f;
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
