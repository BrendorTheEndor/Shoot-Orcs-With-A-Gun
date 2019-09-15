using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour {

    // The target to chase
    [SerializeField] Transform target;
    // The distance from the player the enemy needs to be to activate AI
    [SerializeField] float chaseRange = 5f;

    NavMeshAgent myNavMeshAgent;
    float distanceToTarget = Mathf.Infinity;
    bool chaseTriggered = false;

    void Start() {
        myNavMeshAgent = GetComponent<NavMeshAgent>();
    }

    // If target not in range, do nothing, if target is in range, then chase (can't unagro)
    void Update() {
        distanceToTarget = Vector3.Distance(target.position, transform.position);
        if(distanceToTarget <= chaseRange) {
            chaseTriggered = true;
        }
        if(chaseTriggered) {
            myNavMeshAgent.SetDestination(target.position);
        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
    }
}
