using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ColiisionHandler : MonoBehaviour
{
    [SerializeField] private float levelLoadDelayTime = 1f;

    private AudioSource audioSource;
    private bool isTransitioning = false;

    private void Start()
    {
        audioSource = this.GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision other)
    {
        switch (other.gameObject.tag)
        {
            case "LandingPad":
                StartSuccessSequence();
                break;
            case "Obstacle":
            case "Ground":
                StartCrashSequence();
                break;
        }
    }

    private void StartCrashSequence()
    {
        if (isTransitioning)
        {
            return;
        }

        isTransitioning = true;

        RocketMovement movementScript = this.gameObject.GetComponent<RocketMovement>();
        movementScript.StopThrustingSound(); // Stop playing thrusting sound
        movementScript.enabled = false; // Stop processing player's input when they crashed the rocket

        audioSource.Play();

        // Reload current level
        Invoke("ReloadCurrentLevel", levelLoadDelayTime);
    }

    private void ReloadCurrentLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
        isTransitioning = false;
    }

    private void StartSuccessSequence()
    {
        if (isTransitioning)
        {
            return;
        }

        isTransitioning = true;

        RocketMovement movementScript = this.gameObject.GetComponent<RocketMovement>();
        movementScript.StopThrustingSound(); // Stop playing thrusting sound
        movementScript.enabled = false; // Stop processing player's input when they landed the rocket

        Camera mainCamera = Camera.main;
        AudioSource successAudioSource = mainCamera.transform.GetComponent<AudioSource>();
        successAudioSource.Play();

        Invoke("LoadNextLevel", levelLoadDelayTime);
    }

    private void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings) // The player has finished all the levels
        {
            nextSceneIndex = 0; // Roll back to the first level
        }
        SceneManager.LoadScene(nextSceneIndex);

        isTransitioning = false;
    }
}
