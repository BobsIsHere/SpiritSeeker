using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedSpell : Spell
{
    [SerializeField]
    private float _pushForce = 1500.0f;

    protected override void Awake()
    {
        base.Awake();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Rigidbody enemyRigidBody = other.GetComponent<Rigidbody>();

            if (enemyRigidBody != null)
            {
                Push(enemyRigidBody);
            }

            Destroy(gameObject);
        }
    }

    void Push(Rigidbody enemyRigidBody)
    {
        Vector3 pushDirection = (enemyRigidBody.transform.position - transform.position);
        pushDirection.Normalize();
        pushDirection.y = 0.0f;
        enemyRigidBody.AddForce(pushDirection * _pushForce, ForceMode.Impulse);
    }
}
