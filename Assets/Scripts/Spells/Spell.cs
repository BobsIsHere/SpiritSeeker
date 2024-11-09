using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour
{
    [SerializeField]
    private AudioClip _soundEffect;

    private const string KILL_METHOD = "Kill";

    private float _lifeTime;
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

        _lifeTime += Time.fixedDeltaTime;
    }

    static readonly string[] RAYCAST_MASK = { "Ground", "StaticLevel" };
    bool WallDetection()
    {
        Ray collisionRay = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(collisionRay, Time.fixedDeltaTime * _spellSpeed, LayerMask.GetMask(RAYCAST_MASK)))
        {
            Invoke(KILL_METHOD, _soundEffect.length - _lifeTime);
            DisableMeshRenderer();
            return true;
        }

        return false;
    }

    private void DisableMeshRenderer()
    {
        Transform visuals = transform.Find("Visuals");
        if (visuals != null)
        {
            MeshRenderer meshRenderer = visuals.GetComponent<MeshRenderer>();
            if (meshRenderer != null)
            {
                meshRenderer.enabled = false;
                GetComponent<Collider>().enabled = false;
            }
        }
    }

    protected void Kill()
    {
        Destroy(gameObject);
    }
}
