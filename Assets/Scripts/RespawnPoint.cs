using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPoint : MonoBehaviour
{
    [SerializeField]
    private GameObject _respawnTemplate = null;

    private void OnEnable()
    {
        RespawnPointManager.Instance.RegisterRespawnPoint(this);
    }

    private void OnDisable()
    {
        if(RespawnPointManager.Exists)
        {
            RespawnPointManager.Instance.UnregisterSpawnPoint(this);
        }
    }

    public GameObject Respawn()
    {
        return Instantiate(_respawnTemplate, transform.position, transform.rotation);
    }
}
