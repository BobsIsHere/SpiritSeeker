using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedSpell : Spell
{
    [SerializeField]
    private float _pushForce = 100.0f;

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
                Vector3 pushDirection = (other.transform.position - transform.position);
                pushDirection.Normalize();
                enemyRigidBody.AddForce(pushDirection * _pushForce);
            }

            Destroy(gameObject);
        }
    }
}
