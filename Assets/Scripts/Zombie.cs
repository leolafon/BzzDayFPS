using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private GameObject target;
    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        target = GameObject.Find("Player");
    }

    void LateUpdate()
    {
        navMeshAgent.SetDestination(target.transform.position);
    }
}
