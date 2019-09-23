using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Single class to handle all ammo types, placed on the player, rather than on each weapon
public class Ammo : MonoBehaviour {

    // Slots for each type of ammo
    [SerializeField] AmmoSlot[] ammoSlots;

    [System.Serializable]
    private class AmmoSlot {
        public AmmoType ammoType;
        public int currentAmmo;
    }

    public int GetCurrentAmount(AmmoType ammoType) { return GetAmmoSlot(ammoType).currentAmmo; }

    public void ReduceCurrentAmmo(AmmoType ammoType) { GetAmmoSlot(ammoType).currentAmmo--; }

    public void IncreaseAmmo(AmmoType ammoType, int amount) { GetAmmoSlot(ammoType).currentAmmo += amount; }

    private AmmoSlot GetAmmoSlot(AmmoType ammoType) { // Find out which slot we're referring to
        foreach(AmmoSlot slot in ammoSlots) {
            if(slot.ammoType == ammoType) {
                return slot;
            }
        }
        return null;
    }

}
