using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitcher : MonoBehaviour {

    int currentWeapon = 0;

    void Start() {
        SetWeaponActive();
    }

    void Update() {
        int previousWeapon = currentWeapon;

        ProcessKeyInput();
        ProcessScrollWheelInput();

        if(previousWeapon != currentWeapon) {
            SetWeaponActive();
        }
    }

    private void ProcessKeyInput() {
        if(Input.GetKeyDown(KeyCode.Alpha1)) {
            currentWeapon = 0;
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2)) {
            currentWeapon = 1;
        }
        else if(Input.GetKeyDown(KeyCode.Alpha3)) {
            currentWeapon = 2;
        }
    }

    private void ProcessScrollWheelInput() {
        if(Input.GetAxis("Mouse ScrollWheel") > 0) {
            if(currentWeapon >= transform.childCount - 1) { // Loop around if needed
                currentWeapon = 0;
            }
            else {
                currentWeapon++;
            }
        }
        else if(Input.GetAxis("Mouse ScrollWheel") < 0) {
            if(currentWeapon <= 0) { // Loop around if needed
                currentWeapon = transform.childCount - 1;
            }
            else {
                currentWeapon--;
            }
        }
    }

    private void SetWeaponActive() {
        int weaponIndex = 0;
        foreach(Transform weapon in transform) { // each child
            if(weaponIndex == currentWeapon) {
                weapon.gameObject.SetActive(true);
            }
            else {
                weapon.gameObject.SetActive(false);
            }
            weaponIndex++;
        }
    }
}
