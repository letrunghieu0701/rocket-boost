using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private GameManager() { }

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
    }

    private void Start()
    {
        UIManager.Instance.ShowUI("StartGame_UI");
    }

    private void Update()
    {

    }

    public void StartGame()
    {
        LoadNextLevel();
        UIManager.Instance.HideAllUI(); ;
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
