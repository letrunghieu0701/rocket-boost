using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ColiisionHandler : MonoBehaviour
{
    [SerializeField] private float levelLoadDelayTime = 1f;

    private AudioSource audioSource;
    private bool isTransitioning = false;

    private ParticleSystem successEffect;
    private ParticleSystem crashEffect;

    private void Start()
    {
        audioSource = this.transform.GetComponent<AudioSource>();

        successEffect = this.transform.Find("Effects/Success").GetComponent<ParticleSystem>();
        crashEffect = this.transform.Find("Effects/Explosion").GetComponent<ParticleSystem>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (isTransitioning)
        {
            return;
        }

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
        isTransitioning = true;

        RocketMovement movementScript = this.transform.GetComponent<RocketMovement>();
        movementScript.StopThrusting();
        movementScript.StopSideThrustersParticle();
        movementScript.enabled = false; // Stop processing player's input when they crashed the rocket

        audioSource.Play();

        crashEffect.Play();

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
        isTransitioning = true;

        RocketMovement movementScript = this.transform.GetComponent<RocketMovement>();
        movementScript.StopThrusting();
        movementScript.StopSideThrustersParticle();
        movementScript.enabled = false; // Stop processing player's input when they landed the rocket

        Camera mainCamera = Camera.main;
        AudioSource successAudioSource = mainCamera.transform.GetComponent<AudioSource>();
        successAudioSource.Play();

        successEffect.Play();

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
