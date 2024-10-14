using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField]
    private int _maxLives = 3;

    private int _currentLives;

    public int MaxLives
    {
        get
        {
            return _maxLives;
        }
    }

    public int CurrentLives
    {
        get
        {
            return _currentLives;
        }
    }

    private void Awake()
    {
        _currentLives = _maxLives;
    }
    
    public void TakeDamage(int amount)
    {
        _currentLives -= amount;

        if (_currentLives <= 0)
        {
            Kill();
        }
    }

    void Kill()
    {
        Destroy(gameObject);
    }
}
