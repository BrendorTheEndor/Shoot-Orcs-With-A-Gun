using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class WeaponZoom : MonoBehaviour {

    [SerializeField] Camera mainCamera;
    [SerializeField] RigidbodyFirstPersonController playerController;
    [SerializeField] float zoomAmount = 60f;
    [SerializeField] float zoomPerFrame = 5f;
    [SerializeField] float lowestSensitivity = .5f;

    float initialFOV;
    float initialSensitivity;

    void Start() {
        initialFOV = mainCamera.fieldOfView;
        initialSensitivity = playerController.mouseLook.XSensitivity;
    }

    void Update() {
        if(Input.GetButton("Fire2")) {
            mainCamera.fieldOfView -= zoomPerFrame;
            playerController.mouseLook.XSensitivity = lowestSensitivity;
            playerController.mouseLook.YSensitivity = lowestSensitivity;
        }
        else {
            mainCamera.fieldOfView += zoomPerFrame;
            playerController.mouseLook.XSensitivity = initialSensitivity;
            playerController.mouseLook.YSensitivity = initialSensitivity;
        }

        mainCamera.fieldOfView = Mathf.Clamp(mainCamera.fieldOfView, 90 - zoomAmount, initialFOV);
    }
}
