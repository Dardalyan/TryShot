using UnityEngine;
using UnityEngine.AI;


public class ZombieController : MonoBehaviour
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
        die();
    }


    private void die()
    {
        if (_health <= 0)
        {
            Destroy(gameObject);
        }
    }
}