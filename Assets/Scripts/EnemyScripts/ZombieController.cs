using UnityEngine;
using UnityEngine.AI;


public class ZombieController : MonoBehaviour
{
    private Ray ray;
    private NavMeshAgent _agent;
    private float _health;
    private Transform Player;
    private PlayerHealth target;
    
    
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("player").transform;
        _agent = GetComponent<NavMeshAgent>();
        _health = 100f;
        target = FindObjectOfType<PlayerHealth>();
        InvokeRepeating("HitTarget", 0f, 2f);
    }

    // Update is called once per frame
    void Update()
    {
        die();
    }

    private void HitTarget()
    {
        target.GetComponent<PlayerHealth>().TakeDamage(zDamage());
    }
    
    public float zDamage()
    {
        float distance = Vector3.Distance (transform.position, Player.position);
        if (distance <= 15f)
        {
            return 1f;
        }

        return 0f;
    }

    private void die()
    {
        if (_health <= 0)
        {
            Destroy(gameObject);
        }
    }
}