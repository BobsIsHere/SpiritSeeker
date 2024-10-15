using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spirit : MonoBehaviour
{
    private bool _isSpiritCollected = false;

    public bool IsSpiritCollected()
    {
        return _isSpiritCollected;
    }

    public void CollectSpirit()
    {
        _isSpiritCollected = true;
    }
}
