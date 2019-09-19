using System;
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
    bool enemyTriggered = false;

    void Start() {
        myNavMeshAgent = GetComponent<NavMeshAgent>();
    }

    // If target not in range, do nothing, if target is in range, then chase (can't unaggro)
    void Update() {
        distanceToTarget = Vector3.Distance(target.position, transform.position);
        if(distanceToTarget <= chaseRange) {
            enemyTriggered = true;
        }
        if(enemyTriggered) {
            TriggerEnemy();
        }
    }

    private void TriggerEnemy() {
        if(distanceToTarget >= myNavMeshAgent.stoppingDistance) {
            ChaseTarget();
        }
        if(distanceToTarget <= myNavMeshAgent.stoppingDistance) {
            AttackTarget();
        }
    }

    private void ChaseTarget() {
        myNavMeshAgent.SetDestination(target.position);
    }

    private void AttackTarget() {
        Debug.Log("Player has been hit");
    }


    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
    }
}
