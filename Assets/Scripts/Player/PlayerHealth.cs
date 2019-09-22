using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour {

    [SerializeField] float hitPoints = 100;
    [SerializeField] AudioClip[] painGrunts;
    [SerializeField] Canvas damageTakenCanvas;
    [SerializeField] float canvasDisplayTime = .5f;

    private void Start() {
        damageTakenCanvas.enabled = false;
    }

    public void TakeDamage(float damage) {
        hitPoints -= damage;

        AudioSource.PlayClipAtPoint(painGrunts[Random.Range(0, painGrunts.Length)], transform.position); // Plays random pain sound
        damageTakenCanvas.enabled = true;
        StartCoroutine(WaitAndTurnOffCanvas());

        if(hitPoints <= 0) {
            FindObjectOfType<DeathHandler>().HandleDeath();
        }
    }

    IEnumerator WaitAndTurnOffCanvas() {
        yield return new WaitForSeconds(canvasDisplayTime);
        damageTakenCanvas.enabled = false;
    }
}
