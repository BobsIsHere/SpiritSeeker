using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.VFX;

public class BlueSpell : Spell
{
    [SerializeField]
    private float _freezeDuration = 3.0f;

    private Color _freezeColor = Color.blue;

    protected override void Awake()
    {
        base.Awake();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            NavMeshMovementBehaviour movementBehaviour = other.GetComponent<NavMeshMovementBehaviour>();

            Transform colliderTransform = other.transform.Find("Ghost");
            SkinnedMeshRenderer ghostObj = null;

            if (colliderTransform != null)
            {
                ghostObj = colliderTransform.Find("GhostMesh").GetComponent<SkinnedMeshRenderer>();
            }

            if (movementBehaviour != null)
            {
                movementBehaviour.Freeze(_freezeDuration, ghostObj);
                //StartCoroutine(ChangeColorTemporarily(ghostObj, _freezeDuration));
            }

            Kill();
        }
    }
}
