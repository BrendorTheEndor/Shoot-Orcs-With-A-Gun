using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: Might be worth it to refactor this into a different script for each weapon type
public class Weapon : MonoBehaviour {

    [Header("General Reference")]
    [SerializeField] Camera firstPersonCamera;
    [SerializeField] Ammo ammoSlot;

    [Header("Weapon Attributes")]
    [SerializeField] AmmoType ammoType;
    [SerializeField] float weaponRange = 100f;
    [SerializeField] float minWeaponDamagePerShot = 10f;
    [SerializeField] float maxWeaponDamagePerShot = 10f;
    [SerializeField] float shotDeviationFactor = .05f;
    [SerializeField] float timeBetweenShots = 1f;

    [Header("Effects")]
    [SerializeField] ParticleSystem muzzleFlash;
    //[SerializeField] ParticleSystem bulletTrail;
    [SerializeField] AudioClip fireSFX;
    [SerializeField] float fireVolume = 1f;
    [SerializeField] GameObject hitEffect;


    // Used to cap fire rate
    Coroutine firingCorutine;
    Animator gunAnimator;
    bool canFire = true;

    // Used for animations
    const string FIRING_BOOL = "isShooting";

    void Start() {
        gunAnimator = GetComponent<Animator>();
        gunAnimator.SetBool(FIRING_BOOL, false);
    }

    // Just used for weapon switching
    private void OnEnable() {
        gunAnimator = GetComponent<Animator>();
        gunAnimator.SetBool(FIRING_BOOL, false);
        canFire = true;
    }

    void Update() {
        if(ammoType == AmmoType.SmallBullets) {
            ProcessAutoFire();
        }
        else if(ammoType == AmmoType.LargeBullets) {
            ProcessSingleShot();
        }
        else if(ammoType == AmmoType.Shells) {
            ProcessShotgunSpread();
        }
    }

    private void ProcessAutoFire() {
        if(Input.GetButtonDown("Fire1")) {
            firingCorutine = StartCoroutine(FireContinuously());
        }
        if(Input.GetButtonUp("Fire1")) {
            gunAnimator.SetBool(FIRING_BOOL, false);
            // To protect against null reference
            if(ammoSlot.GetCurrentAmount(ammoType) > 0) {
                StopCoroutine(firingCorutine);
            }
        }
    }

    private void ProcessSingleShot() {
        if(Input.GetButtonDown("Fire1") && canFire && (ammoSlot.GetCurrentAmount(ammoType) > 0)) {
            PlayFX();
            ProcessShot();
            ammoSlot.ReduceCurrentAmmo(ammoType);
            canFire = false;
            StartCoroutine(DelayFire());
        }
        else {
            gunAnimator.SetBool(FIRING_BOOL, false);
        }
    }

    private void ProcessShotgunSpread() {
        if(Input.GetButtonDown("Fire1") && canFire && (ammoSlot.GetCurrentAmount(ammoType) > 0)) {
            PlayFX();

            for(int i = 0; i < 6; i++) {
                ProcessShot();
            }

            ammoSlot.ReduceCurrentAmmo(ammoType);
            canFire = false;
            StartCoroutine(DelayFire());
        }
        else {
            gunAnimator.SetBool(FIRING_BOOL, false);
        }
    }

    // Methods both auto fire and single shot need

    private void PlayFX() {
        gunAnimator.SetBool(FIRING_BOOL, true);
        muzzleFlash.Play();
        //bulletTrail.Play();
        AudioSource.PlayClipAtPoint(fireSFX, Camera.main.transform.position, fireVolume);
    }

    private void ProcessShot() {

        // What we hit with the cast
        RaycastHit hit;

        Vector3 shotDeviation = new Vector3(UnityEngine.Random.Range(-shotDeviationFactor, shotDeviationFactor),
            UnityEngine.Random.Range(-shotDeviationFactor, shotDeviationFactor), UnityEngine.Random.Range(-shotDeviationFactor, shotDeviationFactor));

        // First is where to shoot the ray from, next is what direction, then what we hit, and finally the range
        if(Physics.Raycast(firstPersonCamera.transform.position, firstPersonCamera.transform.forward + shotDeviation, out hit, weaponRange)) { // If we hit something
            CreateHitImpact(hit);
            EnemyHealth target = hit.transform.GetComponent<EnemyHealth>();
            if(target == null) { return; } // protects against null reference
            target.TakeDamage(UnityEngine.Random.Range(minWeaponDamagePerShot, maxWeaponDamagePerShot));
        }
        else { return; } // protects against null reference
    }

    private void CreateHitImpact(RaycastHit hit) {
        // Make the hit effect, at the point where hit, in direction of normal of the hit
        GameObject impact = Instantiate(hitEffect, hit.point, Quaternion.LookRotation(hit.normal));
        Destroy(impact, .1f);
    }

    // Only auto fire

    IEnumerator FireContinuously() {
        while(true) { // Infinite loop to always fire while button held, but delay by a specified amount

            if(ammoSlot.GetCurrentAmount(ammoType) <= 0) {
                gunAnimator.SetBool(FIRING_BOOL, false);
                yield break;
            }

            PlayFX();
            ProcessShot();
            ammoSlot.ReduceCurrentAmmo(ammoType);
            yield return new WaitForSeconds(timeBetweenShots);
        }
    }

    // Only single shot

    IEnumerator DelayFire() {
        yield return new WaitForSeconds(timeBetweenShots);
        canFire = true;
    }
}
