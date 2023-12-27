using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAnimationContorller : MonoBehaviour
{
    private Animator animator;
    private GameObject player;
    private NavMeshAgent agent;
    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("user");
    }

    void Update()
    {
        float distance = Vector3.Distance (transform.position, player.transform.position);
        if (agent != null)
        {
            animator.SetBool("isOnTarget",GetComponent<enemyController>().isOnTarget());
        }
        
        
        
        /*
         if ((distance < 90 &&  GetComponent<enemyController>().isOnTarget())
               || GetComponent<enemyController>().isOnTarget())
           {
               animator.SetBool("isOnTarget",GetComponent<enemyController>().isOnTarget());
           }
         */
    }
}
