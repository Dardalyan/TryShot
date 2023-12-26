using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationContorller : MonoBehaviour
{
    private Animator animator;
    private GameObject player;
    void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.Find("user");
    }

    void Update()
    {
        float distance = Vector3.Distance (transform.position, player.transform.position);
        animator.SetBool("isOnTarget",GetComponent<enemyController>().isOnTarget());
        /*
         if ((distance < 90 &&  GetComponent<enemyController>().isOnTarget())
               || GetComponent<enemyController>().isOnTarget())
           {
               animator.SetBool("isOnTarget",GetComponent<enemyController>().isOnTarget());
           }
         */
    }
}
