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
        // TODO check the distance from the player, and make it miss if they aren't close enough. Also, a sound effect for when an attack lands would be good feedback
        target.TakeDamage(damageToDeal);
    }
}
