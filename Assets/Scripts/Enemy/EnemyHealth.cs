using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour {

    [SerializeField] float hitPoints = 100f;

    const string DAMAGE_METHOD = "OnDamageTaken";
    
    public void TakeDamage(float damageToTake) {
        hitPoints -= damageToTake;

        BroadcastMessage(DAMAGE_METHOD); // Calls this method on this game object or children

        if(hitPoints <= 0) {
            Destroy(gameObject);
        }
    }
}
