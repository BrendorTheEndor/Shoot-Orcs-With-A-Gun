using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : MonoBehaviour {

    [SerializeField] int pickupAmount = 5;
    [SerializeField] AmmoType ammoType;

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Player") {
            Debug.Log("Pickup gotten");
            other.GetComponent<Ammo>().IncreaseAmmo(ammoType, pickupAmount);
            Destroy(gameObject);
        }
    }

}
