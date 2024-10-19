using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedSpell : Spell
{
    [SerializeField]
    private float pushForce = 10.0f;

    protected override void Start()
    {
        base.Start();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Rigidbody enemyRigidBody = other.GetComponent<Rigidbody>();

            if(enemyRigidBody != null)
            {
                Vector3 pushDirection = other.transform.position - transform.position;
                enemyRigidBody.AddForce(pushDirection.normalized * pushForce);
            }

            Destroy(gameObject);
        }
    }
}
