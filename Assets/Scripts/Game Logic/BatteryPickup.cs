using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryPickup : MonoBehaviour {

    [SerializeField] float restoreAngle = 90f;
    [SerializeField] float intensityToAdd = 1f;

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Player") {
            other.GetComponentInChildren<FlashlightSystem>().RestoreLightAngle(restoreAngle);
            other.GetComponentInChildren<FlashlightSystem>().RestoreLightIntensity(intensityToAdd);
            Destroy(gameObject);
        }
    }

}
