using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// Script to handle enemy AI and pathing
public class EnemyAI : MonoBehaviour {

    // The distance from the player the enemy needs to be to activate AI
    [SerializeField] float chaseRange = 5f;
    [SerializeField] float turnSpeed = 5f;

    float distanceToTarget = Mathf.Infinity;
    bool enemyTriggered = false;

    Transform target;
    NavMeshAgent myNavMeshAgent;
    Animator myAnimator;
    EnemyHealth enemyHealth;

    // string refs for animator
    const string CHASE_TRIGGER = "beginChase";
    const string IDLE_TRIGGER = "deaggro";
    const string ATTACK_BOOL = "isAttacking";

    void Start() {
        myNavMeshAgent = GetComponent<NavMeshAgent>();
        myAnimator = GetComponent<Animator>();
        enemyHealth = GetComponent<EnemyHealth>();
        target = FindObjectOfType<PlayerHealth>().transform;
    }

    // If target not in range, do nothing, if target is in range, then chase (can't unaggro)
    void Update() {
        if(enemyHealth.IsDead()) {
            enabled = false; // Turns off this component
            myNavMeshAgent.enabled = false; // Disables movement
            return;
        }

        distanceToTarget = Vector3.Distance(target.position, transform.position);
        if(distanceToTarget <= chaseRange) {
            enemyTriggered = true;
        }
        if(enemyTriggered) {
            PursuePlayer();
        }
    }

    // Triggers enemy if damage is taken, is called from a separate script which deals with enemy health
    public void OnDamageTaken() {
        enemyTriggered = true;
    }

    private void PursuePlayer() {

        FaceTaget();

        if(distanceToTarget >= myNavMeshAgent.stoppingDistance) {
            ChaseTarget();
        }
        if(distanceToTarget <= myNavMeshAgent.stoppingDistance) {
            AttackTarget();
        }
    }

    // Changes the rotation of the enemy to face the target
    private void FaceTaget() {
        Vector3 direction = (target.position - transform.position).normalized; // Get the direction to face
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z)); // Find the rotation to look in
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed); // Gradually turn to that rotation
    }

    private void ChaseTarget() {
        myAnimator.SetBool(ATTACK_BOOL, false);
        myAnimator.SetTrigger(CHASE_TRIGGER);
        myNavMeshAgent.SetDestination(target.position);
    }

    private void AttackTarget() {
        myAnimator.SetBool(ATTACK_BOOL, true);
    }

    // Used to show the chase range in the editor
    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
    }

}
