using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicCharacter : MonoBehaviour
{
    protected MovementBehaviour _movementBehaviour;
    protected AttackBehaviour _attackBehaviour;

    protected virtual void Awake()
    {
        _movementBehaviour = GetComponent<MovementBehaviour>();
        _attackBehaviour = GetComponent<AttackBehaviour>();
    }
}
