using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FindPlayer : MonoBehaviour
{

    private NavMeshAgent _agent;
    private Transform Player;
    // Start is called before the first frame update
    void Start()
    {

        _agent = GetComponent<NavMeshAgent>();
        Player = GameObject.Find("user").transform;

    }

    // Update is called once per frame
    void Update()
    {
        _agent.SetDestination(Player.position);
        if (_agent.GetComponent<enemyController>().isOnTarget())
        {
            _agent.stoppingDistance = 100f;
        }

    }
}
