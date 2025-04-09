using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField]
    private GameObject _uiManagerPrefab;

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
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        if (_uiManagerPrefab == null)
        {
            Debug.LogError("GameManager: Missing reference to UIManager prefab");
            return;
        }

        Instantiate(_uiManagerPrefab);

        // Check for open game from testing scene, 
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (currentSceneIndex != 0)
        {
            Debug.Log("<color=green>Non-init scene detected, skipping StartGame UI</color>");
            return;
        }

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
