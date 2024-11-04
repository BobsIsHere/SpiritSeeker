using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour
{
    private const string KILL_METHOD = "Kill";

    protected float _spellSpeed = 20.0f;
    protected float _spellDuration = 5.0f;

    protected virtual void Awake()
    {
        Invoke(KILL_METHOD, _spellDuration);
    }

    protected virtual void FixedUpdate()
    {
        if (!WallDetection())
        {
            transform.position += transform.forward * _spellSpeed * Time.fixedDeltaTime;
        }
    }

    static readonly string[] RAYCAST_MASK = { "Ground", "StaticLevel" };
    bool WallDetection()
    {
        Ray collisionRay = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(collisionRay, Time.fixedDeltaTime * _spellSpeed, LayerMask.GetMask(RAYCAST_MASK)))
        {
            Kill();
            return true;
        }

        return false;
    }

    private void Kill()
    {
        Destroy(gameObject);
    }
}
