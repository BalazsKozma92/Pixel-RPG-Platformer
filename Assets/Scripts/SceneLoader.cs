using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] string sceneName;
    [SerializeField] float loadDelay = 1f;

    bool playerNear = false;

    void Update() {
        if (Keyboard.current.upArrowKey.isPressed && playerNear) {
            StartCoroutine(LoadNextScene(sceneName));
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            playerNear = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            playerNear = false;
        }
    }

    IEnumerator LoadNextScene(string name) {
        yield return new WaitForSeconds(loadDelay);
        SceneManager.LoadScene(name);
    }
}
