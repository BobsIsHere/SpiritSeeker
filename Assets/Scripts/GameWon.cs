using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameWon : MonoBehaviour
{
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
        SceneManager.LoadScene(0);
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
