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
    private InputActionReference _hideAction;

    private Health _playerHealth;

    private Rigidbody _rigidbody;

    private bool _isHiding = false;

    private const float _coolDownTimer = 0.5f;
    private float _currentCoolDownTimer = 0.0f;

    private const string SPIRIT_TAG = "Spirit";
    private const string ENEMY_TAG = "Enemy";
    private const string COVER_TAG = "Cover";

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
        HandleHideInput();

        _currentCoolDownTimer += Time.deltaTime;
    }

    void HandleMovementInput()
    {
        if (_movementBehaviour == null || _movementAction == null)
        {
            return;
        }

        if (!_isHiding)
        {
            //movement
            Vector2 movementInput = _movementAction.action.ReadValue<Vector2>();
            Vector3 movement = new Vector3(movementInput.x, 0, movementInput.y);
            _movementBehaviour.DesiredMovementDirection = movement;
        }
    }

    void HandleSpellInput()
    {
        if (_attackBehaviour.GetMagicStaff() == null && _attackBehaviour == null)
        {
            return;
        }

        if (_castSpellAction.action.triggered)
        {
            Debug.Log("Casting spell");
            _attackBehaviour.Attack();
        }

        if (_switchSpellAction.action.triggered)
        {
            Debug.Log("Switching spell");
            _attackBehaviour.GetMagicStaff().SwitchSpell();
        }
    }

    void HandleHideInput()
    {
        if (_hideAction.action.triggered && IsNearCover())
        {
            ToggleHide();
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

    private bool IsNearCover()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 1.0f);

        foreach (Collider collider in colliders)
        {
            if (collider.tag == COVER_TAG)
            {
                return true;
            }
        }

        return false;
    }

    private void ToggleHide()
    {
        _isHiding = !_isHiding;
        gameObject.SetActive(!_isHiding);

        if (_isHiding)
        {
            // Disable all player actions
            _movementBehaviour.enabled = false;

            // disable magic
        }
        else
        {
            // Enable all player actions
            _movementBehaviour.enabled = true;

            // enable magic
        }
    }
}
