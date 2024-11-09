using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    [SerializeField]
    private GameObject _player = null;

    private void Update()
    {
        if(_player == null)
        {
            OnPlayerDeath();
        }
    }

    private void OnPlayerDeath()
    {
        Debug.Log("Game Over");
        SceneManager.LoadScene("SpiritSeeker");
    }
}
