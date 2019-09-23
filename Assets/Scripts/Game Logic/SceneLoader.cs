using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {

    private void Awake() {
        Cursor.lockState = CursorLockMode.None; // Don't lock the cursor anymore, basically let the people click the buttons
        Cursor.visible = true;
    }

    public void ReloadScene() {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame() {
        Application.Quit();
    }

    public void LoadNextScene() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void RestartGame() {
        SceneManager.LoadScene(0);
    }

    private void OnTriggerEnter(Collider other) {
        LoadNextScene();
    }
}
