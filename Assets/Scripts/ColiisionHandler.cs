using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ColiisionHandler : MonoBehaviour
{
    [SerializeField] private float levelLoadDelayTime = 1f;

    private void OnCollisionEnter(Collision other)
    {
        switch (other.gameObject.tag)
        {
            case "LandingPad":
                StartSuccessSequence();
                break;
            case "Obstacle":
                StartCrashSequence();
                break;
            case "Ground":
                StartCrashSequence();
                break;
        }
    }

    private void StartCrashSequence()
    {
        RocketMovement movementScript = this.gameObject.GetComponent<RocketMovement>();
        movementScript.StopThrustingSound(); // Stop playing sound
        movementScript.enabled = false; // Stop processing player's input when they crashed the rocket

        // Reload current level
        Invoke("ReloadCurrentLevel", levelLoadDelayTime);
    }

    private void ReloadCurrentLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    private void StartSuccessSequence()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings) // The player has finished all the levels
        {
            Debug.Log("You Win The Game");
            RocketMovement movementScript = this.gameObject.GetComponent<RocketMovement>();
            movementScript.enabled = false; // Stop processing player's input when they win the game
            return;
        }

        StartCoroutine("LoadNextLevel");
    }

    private IEnumerator LoadNextLevel()
    {
        yield return new WaitForSeconds(levelLoadDelayTime);
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        SceneManager.LoadScene(nextSceneIndex);
    }
}
