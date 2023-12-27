using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FindPlayer : MonoBehaviour
{

    private NavMeshAgent _agent;
    private Transform Player;
    private Animator animator;

    void Start()
    {

        _agent = GetComponent<NavMeshAgent>();
        Player = GameObject.Find("player").transform;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
        float distance = Vector3.Distance (transform.position, Player.position);
        

        if (GetComponent<enemyController>().isOnTarget())
        {
            _agent.destination = _agent.transform.position;
            _agent.isStopped = true;
            animator.SetBool("isOnTarget", true);
        }
        else
        {
            if (distance < 75f)
            {
                _agent.destination = _agent.transform.position;
                _agent.isStopped = true;
                animator.SetBool("isOnTarget", true);
            }
            else
            {
                _agent.destination = Player.position;
                animator.SetBool("isOnTarget", false);
            }

        }
        
    }
}
