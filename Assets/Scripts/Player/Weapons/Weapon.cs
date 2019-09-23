using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/* Script applied to each weapon the player can use; handles several types of weapons
 * Somewhat light on comments, but the method/variable names are intentionally very descriptive as to be self-commenting
 */
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
    [SerializeField] AudioClip fireSFX;
    [SerializeField] float fireVolume = 1f;
    [SerializeField] GameObject hitEffect;
    [SerializeField] TextMeshProUGUI ammoDisplayText;


    // Used to cap fire rate
    Coroutine firingCorutine;
    Animator gunAnimator;
    bool canFire = true;

    // Used for animations
    const string FIRING_BOOL = "isShooting";

    //------------------------------------------------------------------------------------------------------------------------

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

        DisplayAmmo();

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

    //------------------------------------------------------------------------------------------------------------------------

    // Process the different firing types

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

            // Loops 6 times to fire 6 random spread shots
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

    //------------------------------------------------------------------------------------------------------------------------

    // Methods all types need

    private void PlayFX() {
        gunAnimator.SetBool(FIRING_BOOL, true);
        muzzleFlash.Play();
        AudioSource.PlayClipAtPoint(fireSFX, Camera.main.transform.position, fireVolume);
    }

    private void ProcessShot() {

        // What we hit with the cast
        RaycastHit hit;

        // Gets a random Vector3 to add to the center of the screen, so that not all shots are 100% accurate
        Vector3 shotDeviation = new Vector3(UnityEngine.Random.Range(-shotDeviationFactor, shotDeviationFactor),
            UnityEngine.Random.Range(-shotDeviationFactor, shotDeviationFactor), UnityEngine.Random.Range(-shotDeviationFactor, shotDeviationFactor));

        // First is where to shoot the ray from, next is what direction, then what we hit, and finally the range
        if(Physics.Raycast(firstPersonCamera.transform.position, firstPersonCamera.transform.forward + shotDeviation, out hit, weaponRange)) { // If we hit something, and assigns this to "hit"
            CreateHitImpact(hit);
            EnemyHealth target = hit.transform.GetComponent<EnemyHealth>();
            if(target == null) { return; } // protects against null reference
            target.TakeDamage(UnityEngine.Random.Range(minWeaponDamagePerShot, maxWeaponDamagePerShot)); // Simple damage spread
        }
        else { return; } // protects against null reference
    }

    private void CreateHitImpact(RaycastHit hit) {
        // Make the hit effect, at the point where hit, in direction of normal of the hit
        GameObject impact = Instantiate(hitEffect, hit.point, Quaternion.LookRotation(hit.normal));
        Destroy(impact, .1f);
    }

    private void DisplayAmmo() {
        ammoDisplayText.text = ammoSlot.GetCurrentAmount(ammoType).ToString();
    }

    //------------------------------------------------------------------------------------------------------------------------

    // Only auto fire

    IEnumerator FireContinuously() {
        while(true) { // Infinite loop to always fire while button held, but delay by a specified amount

            // Used to prevent the firing animation from continuing if ammo runs out and the button is still held down
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

    //------------------------------------------------------------------------------------------------------------------------

    // Only single shot

    IEnumerator DelayFire() {
        yield return new WaitForSeconds(timeBetweenShots);
        canFire = true;
    }
}
