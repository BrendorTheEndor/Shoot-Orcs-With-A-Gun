using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour {

    [SerializeField] float hitPoints = 100f;

    bool isDead = false;

    const string DAMAGE_METHOD = "OnDamageTaken";
    const string DIE_TRIGGER = "Die";

    public void TakeDamage(float damageToTake) {
        hitPoints -= damageToTake;

        BroadcastMessage(DAMAGE_METHOD); // Calls this method on this game object or children

        if(hitPoints <= 0) {
            Die();
        }
    }

    private void Die() {
        if(isDead) { return; }
        GetComponent<CapsuleCollider>().enabled = false; // They're really big, so this is to prevent the bodies from getting in the way
        GetComponent<Animator>().SetTrigger(DIE_TRIGGER);
        isDead = true;
    }

    public bool IsDead() { return isDead; }
}
