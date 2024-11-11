using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GlowStick : MonoBehaviour
{
    [SerializeField]
    private float _speed = 30.0f;

    [SerializeField]
    private float _lifeTime = 5.0f;

    private Rigidbody _rigidBody;

    private Vector3 _direction;
    private bool _hasDirection = false;

    private const string DESTROY_METHOD = "DestroyGlowStick";

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
        Invoke(DESTROY_METHOD, _lifeTime);
    }

    private void Start()
    {
        if (_hasDirection)
        {
            _rigidBody.velocity = _direction * _speed;

            _rigidBody.drag = 0.1f;
            _rigidBody.angularDrag = 0.05f;
        }
        else
        {
            Debug.LogWarning("GlowStick has no direction set.");
        }
    }

    public void SetDirection(Vector3 direction)
    {
        _direction = direction;
        _hasDirection = true;
    }

    private void DestroyGlowStick()
    {
        Destroy(gameObject);
    }
}
