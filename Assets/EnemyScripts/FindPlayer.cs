using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FindPlayer : MonoBehaviour
{

    public NavMeshAgent Enemy;
    public Transform Player;
    // Start is called before the first frame update
    void Start(){
    


    }

    // Update is called once per frame
    void Update()
    {
        Enemy.SetDestination(Player.position);
        if (Enemy.GetComponent<enemyController>().isOnTarget())
        {
            Enemy.stoppingDistance = 100f;
        }
        else
        {
            Enemy.stoppingDistance = 20f;
        }
    }
}
