using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ColiisionHandler : MonoBehaviour
{
    [SerializeField] private float levelLoadDelayTime = 1f;

    private AudioSource _audioSource;
    private bool _isTransitioningScene = false;

    private ParticleSystem _successEffect;
    private ParticleSystem _crashEffect;

    private bool _isCheckingCollision = true;

    private void Start()
    {
        _audioSource = this.transform.GetComponent<AudioSource>();

        _successEffect = this.transform.Find("Effects/Success").GetComponent<ParticleSystem>();
        _crashEffect = this.transform.Find("Effects/Explosion").GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        ProcessDebugKeys();
    }

    private void ProcessDebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.L)) // Load next level
        {
            LoadNextLevel();
        }
        if (Input.GetKeyDown(KeyCode.C)) // Turn off/on collision checking
        {
            _isCheckingCollision = !_isCheckingCollision;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (_isTransitioningScene || !_isCheckingCollision)
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
        _isTransitioningScene = true;
        UIManager.Instance.ShowUI("GameOver_UI");

        RocketMovement movementScript = this.transform.GetComponent<RocketMovement>();
        movementScript.StopThrusting();
        movementScript.StopRotating();
        movementScript.enabled = false; // Stop processing player's input when they crashed the rocket

        _audioSource.Play();

        _crashEffect.Play();

        // Reload current level
        Invoke("ReloadCurrentLevel", levelLoadDelayTime);
    }

    private void ReloadCurrentLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
        _isTransitioningScene = false;
        UIManager.Instance.HideAllUI();
    }

    private void StartSuccessSequence()
    {
        _isTransitioningScene = true;

        RocketMovement movementScript = this.transform.GetComponent<RocketMovement>();
        movementScript.StopThrusting();
        movementScript.StopRotating();
        movementScript.enabled = false; // Stop processing player's input when they landed the rocket

        Camera mainCamera = Camera.main;
        AudioSource successAudioSource = mainCamera.transform.GetComponent<AudioSource>();
        successAudioSource.Play();

        _successEffect.Play();

        Invoke("LoadNextLevel", levelLoadDelayTime);
    }

    private void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings) // The player has finished all the levels
        {
            //nextSceneIndex = 0; // Roll back to the first level
            UIManager.Instance.ShowUI("GameFinish_UI");
            return;
        }
        SceneManager.LoadScene(nextSceneIndex);

        _isTransitioningScene = false;
    }
}
