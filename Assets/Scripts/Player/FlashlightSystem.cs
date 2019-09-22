using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightSystem : MonoBehaviour {

    [SerializeField] float lightDecay = .1f;
    [SerializeField] float angleDecay = 1f;
    [SerializeField] float minAngle = 40f;

    Light myLight;

    private void Start() {
        myLight = GetComponent<Light>();
    }

    private void Update() {
        DecreaseLightAngle();
        DecreaseLightIntensity();
    }

    private void DecreaseLightAngle() {
        if(myLight.spotAngle <= minAngle) {
            return;
        }
        else {
            myLight.spotAngle -= angleDecay * Time.deltaTime;
        }
    }

    private void DecreaseLightIntensity() {
        myLight.intensity -= lightDecay * Time.deltaTime;
    }

    public void RestoreLightAngle(float angleToRestore) {
        myLight.spotAngle = angleToRestore;
    }

    public void RestoreLightIntensity(float intensityAmount) {
        myLight.intensity += intensityAmount;
    }
}
