using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
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

    [SerializeField]
    private UnityEvent _onDamageEvent;

    private Health _playerHealth;

    private Rigidbody _rigidbody; 

    private bool _isHiding = false;

    private const float COOLDOWN_TIMER = 0.5f;
    private float _currentCoolDownTimer = 0.0f;

    private const string SPIRIT_TAG = "Spirit";
    private const string ENEMY_TAG = "Enemy";
    private const string COVER_TAG = "Cover";

    private const float COVER_RADIUS = 5.0f;
    private const float HIDE_DURATION = 5.0f;

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
        if (!_isHiding)
        {
            HandleMovementInput();
            HandleSpellInput();
        }

        HandleHideInput();

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
            if (_isHiding)
                return;

            if (_currentCoolDownTimer >= COOLDOWN_TIMER)
            {
                _currentCoolDownTimer = 0.0f;

                ResetPosition();
                _onDamageEvent?.Invoke();
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
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, COVER_RADIUS);
        foreach (Collider collider in hitColliders)
        {
            Debug.Log("Detected object: " + collider.gameObject.name + " with tag: " + collider.tag);
            if (collider.CompareTag(COVER_TAG))
            {
                Debug.Log("Cover object detected: " + collider.gameObject.name);
                return true;
            }
        }
        return false;
    }

    private void ToggleHide()
    {
        _movementBehaviour.DesiredMovementDirection = new Vector3(0, 0, 0);

        GetComponent<Collider>().enabled = _isHiding;

        Transform visuals = transform.Find("Visuals");
        if (visuals != null) 
        {
            ToggleMeshRenderers(visuals);
        }

        // Switch hiding state
        _isHiding = !_isHiding;
    }

    private void ToggleMeshRenderers(Transform parent)
    {
        // Disable mesh renderer if it exist
        MeshRenderer meshRenderer = parent.GetComponent<MeshRenderer>();
        if (meshRenderer != null)
        {
            meshRenderer.enabled = _isHiding;
        }

        // Iterate through its children
        foreach (Transform child in parent)
        {
            // Recursively toggle mesh renderers
            ToggleMeshRenderers(child);
        }
    }

    private IEnumerator HideDuration()
    {
        yield return new WaitForSeconds(HIDE_DURATION);

        if (_isHiding)
        {
            ToggleHide();
        }
    }

    public bool IsHiding()
    {
        return _isHiding;
    }
}
