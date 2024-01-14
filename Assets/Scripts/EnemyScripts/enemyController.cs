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
    
    
    // Start is called before the first frame update
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _health = 100f;
        target = FindObjectOfType<PlayerHealth>();
        InvokeRepeating("HitTarget", 0f, 2f);
    }

    // Update is called once per frame
    void Update()
    {
        var iot = isOnTarget();
        if(iot)
            Debug.Log("On The TARGET !!!");
        fire();
        hitConfirmed();
        die();
    }

    private void HitTarget()
    {
        target.GetComponent<PlayerHealth>().TakeDamage(enemyFireDamage());
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
            //Debug.Log("Fire!");
            return true;
        }   

        if (isOnTarget())
        {
            //Debug.Log("Fire!");
            return true;
        }
        return false;
    }

    public bool hitConfirmed()
    {
        if (fire() && isOnTarget())
        {
            //Debug.Log("Hit Confirmed !");
            return true;
        }

        return false;
    }

    public float enemyFireDamage()
    {
        if (hitConfirmed())
        {
            return 1f;
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
            if (hit.collider.gameObject.name == "PlayerObject")
            {
                state = true;
            }
        }

        return state;

    }
    
    
}
