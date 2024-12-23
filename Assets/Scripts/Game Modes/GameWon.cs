using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameWon : MonoBehaviour
{
    [SerializeField]
    private string _sceneName;

    private const string SPIRIT_TAG = "Spirit";
    private GameObject[] _spirits;

    private void Start()
    {
        _spirits = GameObject.FindGameObjectsWithTag(SPIRIT_TAG);
    }

    private void Update()
    {
        if (AllSpiritsCollected())
        {
            OnGameWon();
        }
    }

    private void OnGameWon()
    {
        Debug.Log("Game Won");
        GameManager.Instance.LoadScene(_sceneName);
    }

    private bool AllSpiritsCollected()
    {
        for(int idx = 0; idx < _spirits.Length; ++idx)
        {
            if (!_spirits[idx].GetComponent<Spirit>().IsSpiritCollected)
            {
                return false;
            }
        }

        return true;
    }
}
