using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCharacter : BasicCharacter
{
    [SerializeField]
    private InputActionAsset _inputAsset;

    [SerializeField]
    private InputActionReference _movementAction;

    protected override void Awake()
    {
        base.Awake();

        if (_inputAsset == null)
        {
            return;
        }
    }

    private void OnEnable()
    {
        if (_inputAsset == null)
        {
            return;
        }

        _inputAsset.Enable();
    }

    private void OnDisable()
    {
        if (_inputAsset == null)
        {
            return;
        }

        _inputAsset.Disable();
    }

    private void Update()
    {
        HandleMovementInput();
    }

    void HandleMovementInput()
    {
        if (_movementBehaviour == null || _movementAction == null)
        {
            return;
        }

        //movement
        Vector2 movementInput = _movementAction.action.ReadValue<Vector2>();
        Vector3 movement = new Vector3(movementInput.x, 0, movementInput.y);
        _movementBehaviour.DesiredMovementDirection = movement;
    }
}
