using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager> {

	private string _currentLevelName = string.Empty;
    List<AsyncOperation> _loadOperations;

    private void Start() {
        DontDestroyOnLoad(gameObject);

        _loadOperations = new List<AsyncOperation>();

        LoadLevel("Main");
    }

    void OnLoadLevelOperationComplete(AsyncOperation ao)
    {
        if (_loadOperations.Contains(ao))
        {
            _loadOperations.Remove(ao);
        }

        Debug.Log("Load Complete");
    }

    void OnUnloadLevelOperationComplete(AsyncOperation ao)
    {
        Debug.Log("Unload Complete");
    }

    public void LoadLevel(string levelName)
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Additive);

        if (ao == null)
        {
            Debug.LogError("[GameManager] Unable to load level " + levelName);
            return;
        }

        ao.completed += OnLoadLevelOperationComplete;
        _loadOperations.Add(ao);

        _currentLevelName = levelName;
    }

    public void UnloadLevel(string levelName)
    {
        AsyncOperation ao = SceneManager.UnloadSceneAsync(levelName);

        if (ao == null)
        {
            Debug.LogError("[GameManager] Unable to unload level " + levelName);
            return;
        }

        ao.completed += OnUnloadLevelOperationComplete;
    }
}
