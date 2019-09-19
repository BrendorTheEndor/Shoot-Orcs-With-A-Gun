using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

    [SerializeField] Camera firstPersonCamera;
    [SerializeField] float weaponRange = 100f;
    [SerializeField] float weaponDamagePerShot = 10f;
    [SerializeField] ParticleSystem muzzleFlash;
    [SerializeField] float fireRate = 1f;

    void Start() {

    }

    void Update() {
        if(Input.GetButtonDown("Fire1")) {
            Shoot();
        }
        else {
            GetComponent<Animator>().SetBool("isShooting", false);
        }
    }

    private void Shoot() {
        PlayVFX();
        ProccessShot();
    }

    private void PlayVFX() {
        GetComponent<Animator>().SetBool("isShooting", true);
        muzzleFlash.Play();
    }

    private void ProccessShot() {
        // What we his with the cast
        RaycastHit hit;

        // First is where to shoot the ray from, next is what direction, then what we hit, and finally the range
        if(Physics.Raycast(firstPersonCamera.transform.position, firstPersonCamera.transform.forward, out hit, weaponRange)) { // If we hit something
            Debug.Log("Hit " + hit.transform.name);
            // TODO: add a hitspark
            EnemyHealth target = hit.transform.GetComponent<EnemyHealth>();

            if(target == null) { return; } // protects against null reference

            target.TakeDamage(weaponDamagePerShot);
        }
        else { return; } // protects against null reference
    }
}
