using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameWon : MonoBehaviour
{
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
        GameObject[] spirits = GameObject.FindGameObjectsWithTag("Spirit");
        return spirits.Length == 0;
    }
}
