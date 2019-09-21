using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour {

    PlayerHealth target;
    [SerializeField] float damageToDeal = 40f;

    void Start() {
        target = FindObjectOfType<PlayerHealth>();
    }

    public void AttackHitEvent() {
        if(target == null) { return; }
        Debug.Log("attack event triggered");
        target.TakeDamage(damageToDeal);
    }
}
