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

    [SerializeField]
    private InputActionReference _castSpellAction;

    [SerializeField]
    private InputActionReference _switchSpellAction;

    [SerializeField]
    private MagicStaff _magicStaff;

    private Health _playerHealth;

    private Rigidbody _rigidbody;

    private const float _coolDownTimer = 0.5f;
    private float _currentCoolDownTimer = 0.0f;

    const string SPIRIT_TAG = "Spirit";
    const string ENEMY_TAG = "Enemy";

    protected override void Awake()
    {
        base.Awake();

        if (_inputAsset == null)
        {
            return;
        }

        _playerHealth = GetComponent<Health>();

        if (_playerHealth == null)
        {
            return;
        }

        _rigidbody = GetComponent<Rigidbody>();

        if (_rigidbody == null)
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
        HandleSpellInput();

        _currentCoolDownTimer += Time.deltaTime;
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

    void HandleSpellInput()
    {
        if (_magicStaff == null && _attackBehaviour == null)
        {
            return;
        }

        if (_castSpellAction.action.triggered)
        {
            _attackBehaviour.Attack();
        }

        if (_switchSpellAction.action.triggered)
        {
            _magicStaff.SwitchSpell();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == SPIRIT_TAG)
        {
            Spirit spirit = other.GetComponent<Spirit>();
            spirit.IsSpiritCollected = true;

            other.gameObject.SetActive(false);
        }
        else if (other.tag == ENEMY_TAG)
        {
            if (_currentCoolDownTimer >= _coolDownTimer)
            {
                _currentCoolDownTimer = 0.0f;

                ResetPosition();
                _playerHealth.TakeDamage(1);
            }
        }
    }

    private void ResetPosition()
    {
        RespawnPoint respawnPoint = RespawnPointManager.Instance.GetNearestRespawnPoint(transform.position);

        if (respawnPoint != null)
        {
            _rigidbody.MovePosition(respawnPoint.transform.position);
        }
    }
}
