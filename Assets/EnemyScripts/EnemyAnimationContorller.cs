using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationContorller : MonoBehaviour
{
    private Animator animator;
    public GameObject player;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float distance = Vector3.Distance (transform.position, player.transform.position);
        if ((distance < 90 &&  GetComponent<enemyController>().isOnTarget())
            || GetComponent<enemyController>().isOnTarget())
        {
            Debug.Log(distance);
        }
    }
}
