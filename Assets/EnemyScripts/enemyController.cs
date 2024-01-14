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
    private PlayerHealth target;
    private float damage = 7.5f;
    
    
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _health = 100f;
        target = FindObjectOfType<PlayerHealth>();
    }
    
    void Update()
    {
        var iot = isOnTarget();
        target.GetComponent<PlayerHealth>().TakeDamage(enemyFireDamage());
        die();
    }
    
    public void TakeDamage(float damage)
    {
        _health -= damage;
    }
    
    private void die()
    {
        if (_health <= 0)
        {
            Destroy(gameObject);
            FindObjectOfType<PointSystem>().AddToPoints();
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
            if (hit.collider.gameObject.name == "player")
            {
                state = true;
            }
        }

        return state;

    }
    
    
}
