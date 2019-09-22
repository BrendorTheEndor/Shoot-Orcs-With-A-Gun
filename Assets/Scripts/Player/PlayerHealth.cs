using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour {

    [SerializeField] float hitPoints = 100;
    [SerializeField] AudioClip[] painGrunts;

    public void TakeDamage(float damage) {
        hitPoints -= damage;

        AudioSource.PlayClipAtPoint(painGrunts[Random.Range(0, painGrunts.Length)], transform.position); // Plays random pain sound

        if(hitPoints <= 0) {
            FindObjectOfType<DeathHandler>().HandleDeath();
        }
    }
}
