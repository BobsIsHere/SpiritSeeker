using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPointManager : MonoBehaviour
{
    #region SINGLETON INSTANCE
    private static RespawnPointManager _instance;

    public static RespawnPointManager Instance
    {
        get
        {
            if (_instance == null && !ApplicationQuitting)
            {
                _instance = FindObjectOfType<RespawnPointManager>();

                if (_instance == null)
                {
                    GameObject newInstance = new GameObject("Singleton_RespawnPointManager");
                    _instance = newInstance.AddComponent<RespawnPointManager>();
                }
            }

            return _instance;
        }
    }

    //Checks if the singleton is alive, useful to reference it when the game is about to
    //close down to avoid memory leaks
    public static bool Exists
    {
        get
        {
            return _instance != null;
        }
    }

    public static bool ApplicationQuitting = false;
    protected virtual void OnApplicationQuit()
    {
        ApplicationQuitting = true;
    }

    #endregion

    private void Awake()
    {
        //we want this object to persist when a scene cchanges
        DontDestroyOnLoad(gameObject);

        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (_instance == this)
        {
            _instance = null;
        }
    }

    private List<RespawnPoint> _respawnPoints = new List<RespawnPoint>();
    public void RegisterRespawnPoint(RespawnPoint respawnPoint)
    {
        if(!_respawnPoints.Contains(respawnPoint))
        {
            _respawnPoints.Add(respawnPoint);
        }
    }

    public void UnregisterSpawnPoint(RespawnPoint respawnPoint)
    {
        _respawnPoints.Remove(respawnPoint);
    }

    private void Update()
    {
        _respawnPoints.RemoveAll(x => x == null);
    }

    public RespawnPoint GetNearestRespawnPoint(Vector3 position)
    {
        RespawnPoint nearestSpawn = null;
        float nearestDistance = float.MaxValue;

        foreach (RespawnPoint respawnPoint in _respawnPoints)
        {
            float distance = Vector3.Distance(position, respawnPoint.transform.position);

            if (distance < nearestDistance)
            {
                nearestSpawn = respawnPoint;
                nearestDistance = distance;
            }
        }

        return nearestSpawn;
    }
}
