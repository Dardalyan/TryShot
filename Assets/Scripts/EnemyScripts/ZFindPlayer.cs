using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZFindPLayer : MonoBehaviour
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
        _agent.SetDestination(Player.position);

        if (distance <= 15f)
        {
            animator.SetBool("isTrigger",true);
            _agent.isStopped = true;
        }
        else
        {
            animator.SetBool("isTrigger",false);
            _agent.isStopped = false;

        }

    }


}