using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }

        ConfigManager.Init();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            LoadNextLevel();
        }
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
    }

    public TData GetConfigData<TKey, TData>(TKey dataID) where TData : class, IConfigData<TKey>
    {
        return ConfigManager.Instance.GetConfigData<TKey, TData>(dataID);
    }
}
