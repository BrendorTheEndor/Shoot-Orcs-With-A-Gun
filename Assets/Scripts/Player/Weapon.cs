using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

    [SerializeField] Camera firstPersonCamera;
    [SerializeField] float weaponRange = 100f;
    [SerializeField] float weaponDamagePerShot = 10f;
    [SerializeField] ParticleSystem muzzleFlash;
    //[SerializeField] ParticleSystem bulletTrail;
    [SerializeField] float timeBetweenShots = 1f;
    [SerializeField] AudioClip fireSFX;
    [SerializeField] float fireVolume = 1f;
    [SerializeField] GameObject hitEffect;

    // Used to cap fire rate
    Coroutine firingCorutine;

    // Used for animations
    const string FIRING_BOOL = "isShooting";

    void Start() {
        GetComponent<Animator>().SetBool(FIRING_BOOL, false);
    }

    void Update() {
        if(Input.GetButtonDown("Fire1")) {
            firingCorutine = StartCoroutine(FireContinuously());
        }
        if(Input.GetButtonUp("Fire1")) {
            GetComponent<Animator>().SetBool(FIRING_BOOL, false);
            StopCoroutine(firingCorutine);
        }
    }

    private void PlayFX() {
        GetComponent<Animator>().SetBool(FIRING_BOOL, true);
        muzzleFlash.Play();
        //bulletTrail.Play();
        AudioSource.PlayClipAtPoint(fireSFX, Camera.main.transform.position, fireVolume);
    }

    private void ProccessShot() {
        // What we hit with the cast
        RaycastHit hit;

        // First is where to shoot the ray from, next is what direction, then what we hit, and finally the range
        if(Physics.Raycast(firstPersonCamera.transform.position, firstPersonCamera.transform.forward, out hit, weaponRange)) { // If we hit something
            CreateHitImpact(hit);
            EnemyHealth target = hit.transform.GetComponent<EnemyHealth>();
            if(target == null) { return; } // protects against null reference
            target.TakeDamage(weaponDamagePerShot);
        }
        else { return; } // protects against null reference
    }

    private void CreateHitImpact(RaycastHit hit) {
        // Make the hit effect, at the point where hit, in direction of normal of the hit
        GameObject impact = Instantiate(hitEffect, hit.point, Quaternion.LookRotation(hit.normal));
        Destroy(impact, .1f);
    }

    IEnumerator FireContinuously() {
        while(true) { // Infinite loop to always fire while button held, but delay by a specified amount
            PlayFX();
            ProccessShot();
            yield return new WaitForSeconds(timeBetweenShots);
        }
    }
}
