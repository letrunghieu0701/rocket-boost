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

#if UNITY_EDITOR
        // Check for open game from testing scene, 
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (currentSceneIndex != 0)
        {
            Debug.Log("<color=green>Non-init scene detected, skipping StartGame UI</color>");
            return;
        }
#endif

        UIManager.Instance.ShowUI("StartGame_UI");
    }

    private void Update()
    {

    }

    public void StartGame()
    {
        LevelManager.Instance.LoadNextLevel();
        UIManager.Instance.HideAllUI(); ;
    }
}
