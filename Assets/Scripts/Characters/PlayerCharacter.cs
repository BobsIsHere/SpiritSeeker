using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

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
    private InputActionReference _throwAction;

    [SerializeField]
    private UnityEvent _onDamageEvent;

    [SerializeField]
    public UnityEvent<int> _onGlowStickCountChanged;

    [SerializeField]
    private GameObject _glowstickPrefab;

    [SerializeField]
    private Transform _glowStickSocket;

    private Health _playerHealth;

    private Rigidbody _rigidbody;

    private bool _isHiding = false;

    private const int MAX_GLOWSTICKS = 3;
    private int _currentGlowsticks = MAX_GLOWSTICKS;

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
            HandleAimingInput();
            HandleSpellInput();
            HandleGlowStickInput();
        }

        HandleHideInput();

        _currentCoolDownTimer += Time.deltaTime;

    }

    private void HandleMovementInput()
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

    private void HandleAimingInput()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Ground")))
        {
            Vector3 worldMousePosition = hit.point;
            worldMousePosition.y = 0;
            _movementBehaviour.DesiredLookatPoint = worldMousePosition;
        }
    }

    private void HandleSpellInput()
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

    private void HandleHideInput()
    {
        if (_hideAction.action.triggered && IsNearCover())
        {
            ToggleHide();
        }
    }

    private void HandleGlowStickInput()
    {
        if (_throwAction.action.triggered && _currentGlowsticks > 0)
        {
            ThrowGlowStick();
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

    public int GetCurrentAmountOfGlowSticks()
    {
        return _currentGlowsticks;
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

    private void ThrowGlowStick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;
        Vector3 worldMousePosition = Vector3.zero;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Ground")))
        {
            worldMousePosition = hit.point;
            worldMousePosition.y = 0;
            _movementBehaviour.DesiredLookatPoint = worldMousePosition;
        }

        // Calculate direction from player to mouse position
        Vector3 direction = (worldMousePosition - _glowStickSocket.position).normalized;

        // Instantiate glowstick
        GameObject glowstick = Instantiate(_glowstickPrefab, _glowStickSocket.position, Quaternion.identity);

        GlowStick glowStickComponent = glowstick.GetComponent<GlowStick>();
        if (glowStickComponent != null)
        {
            const float upwardForce = 25.0f;

            glowStickComponent.SetDirection(direction);
            glowstick.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0.0f, upwardForce, 0.0f));
        }

        // Reduce glowstick count
        --_currentGlowsticks;

        //Invoke event to change HUD
        _onGlowStickCountChanged?.Invoke(_currentGlowsticks);
    }

    private IEnumerator DestroyGlowStick(GameObject glowStick, float duration)
    {
        yield return new WaitForSeconds(duration);

        if (glowStick != null)
        {
            Destroy(glowStick);
        }
    }
}
