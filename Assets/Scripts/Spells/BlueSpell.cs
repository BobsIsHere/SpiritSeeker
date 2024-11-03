using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueSpell : Spell
{
    [SerializeField]
    private float _freezeDuration = 3.0f;

    protected override void Awake()
    {
        base.Awake();
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            NavMeshMovementBehaviour movementBehaviour = other.GetComponent<NavMeshMovementBehaviour>();

            if(movementBehaviour != null)
            {
                movementBehaviour.Freeze(_freezeDuration);
            }

            Destroy(gameObject);
        }
    }
}
