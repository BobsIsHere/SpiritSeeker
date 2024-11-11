using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spirit : MonoBehaviour
{
    private bool _isSpiritCollected = false;

    public bool IsSpiritCollected
    {
        get
        {
            return _isSpiritCollected;
        }
        set
        {
            _isSpiritCollected = value;
        }
    }
}
