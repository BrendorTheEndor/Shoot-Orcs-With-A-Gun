using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathHandler : MonoBehaviour {

    [SerializeField] Canvas gameOverCanvas;

    void Start() {
        gameOverCanvas.enabled = false;
    }

    public void HandleDeath() {
        gameOverCanvas.enabled = true;
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None; // Don't lock the cursor anymore, basically let the people click the buttons
        Cursor.visible = true;
    }
}
