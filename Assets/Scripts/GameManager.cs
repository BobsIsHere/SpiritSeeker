using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public static string _lastScene;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            _lastScene = SceneManager.GetActiveScene().name;
        }
    }

    public void LoadScene(string sceneName)
    {
        _lastScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(sceneName);
    }
}
