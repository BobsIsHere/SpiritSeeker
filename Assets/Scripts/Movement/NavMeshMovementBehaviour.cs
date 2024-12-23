using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NavMeshMovementBehaviour : MovementBehaviour
{
    private GameObject _player;

    [SerializeField]
    private float _chaseRange = 5.0f;

    private NavMeshAgent _navMeshAgent;
    private Vector3 _previousTargetPosition = Vector3.zero;

    private Vector3 _wanderTarget;

    private float _wanderTimer;
    private float _wanderDuration = 5.0f;
    private float _wanderCooldown = 3.0f;
    private float _freezeEndTime;

    private bool _isWandering = false;
    private bool _isPaused = false;
    private bool _isFrozen = false;
    private bool _isChasing = false;

    private PlayerCharacter _playerScript;

    private GameObject _currentGlowStick;
    private float _glowStickCheckInterval = 1.0f;
    private float _glowStickCheckTimer = 0.0f;
    private float _glowStickRange = 10.0f;

    private Color _originalColor;
    private SkinnedMeshRenderer _enemyRenderer;

    protected override void Awake()
    {
        base.Awake();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.speed = _movementSpeed;

        _player = GameObject.FindGameObjectWithTag("Player");

        if (_player != null)
        {
            _playerScript = _player.GetComponent<PlayerCharacter>();
        }

        StartWandering();
    }

    const float MOVEMENT_EPSILON = 0.25f;

    protected override void FixedUpdate()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, _player.transform.position);

        // Check for glow sticks if none is currently targeted
        if (_currentGlowStick == null)
        {
            _glowStickCheckTimer += Time.fixedDeltaTime;
            if (_glowStickCheckTimer >= _glowStickCheckInterval)
            {
                _glowStickCheckTimer = 0.0f;
                _currentGlowStick = FindNearestGlowStick();
            }
        }

        // If a glow stick is found within range, move towards it
        if (_currentGlowStick != null)
        {
            float distanceToGlowStick = Vector3.Distance(transform.position, _currentGlowStick.transform.position);
            if (distanceToGlowStick <= _glowStickRange)
            {
                MoveTowardsGlowStick();
            }
            else
            {
                _currentGlowStick = null;
                _navMeshAgent.stoppingDistance = 0.0f;
            }
        }
        // If player is not hiding and is within chase range, start chasing
        else if (distanceToPlayer <= _chaseRange && !_playerScript.IsHiding())
        {
            if (!_isChasing)
            {
                StartChasing();
            }

            ChasePlayer();
        }
        else
        {
            if (_isChasing)
            {
                StopChasing();
            }

            HandleWandering();
        }
    }

    private GameObject FindNearestGlowStick()
    {
        GameObject[] glowSticks = GameObject.FindGameObjectsWithTag("GlowStick");
        GameObject nearestGlowStick = null;
        float nearestDistance = Mathf.Infinity;

        // Iterate through all glows sticks & find nearest one
        foreach (GameObject glowStick in glowSticks)
        {
            // Check if this glow stick is closer than previously found one & within range
            float distance = Vector3.Distance(transform.position, glowStick.transform.position);
            if (distance < nearestDistance && distance <= _glowStickRange)
            {
                nearestGlowStick = glowStick;
                nearestDistance = distance;
            }
        }

        return nearestGlowStick;
    }

    private void MoveTowardsGlowStick()
    {
        if (_currentGlowStick != null)
        {
            _navMeshAgent.stoppingDistance = 3.0f;
            _navMeshAgent.SetDestination(_currentGlowStick.transform.position);
            _navMeshAgent.isStopped = false;
        }
    }

    private void HandleWandering()
    {
        if (_isWandering)
        {
            _wanderTimer += Time.fixedDeltaTime;

            if (_wanderTimer >= _wanderDuration)
            {
                StopWandering();
            }
            else if ((_wanderTarget - transform.position).sqrMagnitude < MOVEMENT_EPSILON)
            {
                SetNewWanderTarget();
            }
        }
        else if (_isPaused)
        {
            _wanderTimer += Time.fixedDeltaTime;
            if (_wanderTimer >= _wanderCooldown)
            {
                StartWandering();
            }
        }
    }

    private void StartWandering()
    {
        _isWandering = true;
        _isPaused = false;
        _isChasing = false;
        _wanderTimer = 0.0f;
        SetNewWanderTarget();
    }

    private void StopWandering()
    {
        _isWandering = false;
        _isPaused = true;
        _wanderTimer = 0.0f;
        _navMeshAgent.isStopped = true;
    }

    private void SetNewWanderTarget()
    {
        Vector3 randomDirection = Random.insideUnitSphere * 10.0f;
        randomDirection += transform.position;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, 10.0f, NavMesh.AllAreas))
        {
            _wanderTarget = hit.position;
            _navMeshAgent.SetDestination(_wanderTarget);
            _navMeshAgent.isStopped = false;
        }
    }

    public void Freeze(float freezeDuration, SkinnedMeshRenderer enemyRenderer)
    {
        if(!_isFrozen)
        {
            _isFrozen = true;
            _freezeEndTime = Time.time + freezeDuration;
            _navMeshAgent.isStopped = true;

            _enemyRenderer = enemyRenderer;
            Material material = _enemyRenderer.material;
            _originalColor = material.GetColor("_MainColor");

            material.SetColor("_MainColor", Color.blue);

            StartCoroutine(UnfreezeAfterDuration(freezeDuration));
        }
    }

    private IEnumerator UnfreezeAfterDuration(float duration)
    {
        yield return new WaitForSeconds(duration);

        Unfreeze();
    }

    private void Unfreeze()
    {
        _isFrozen = false;
        _navMeshAgent.isStopped = false;

        if (_enemyRenderer != null)
        {
            Material material = _enemyRenderer.material;
            material.SetColor("_MainColor", _originalColor);
        }
    }

    private void StartChasing()
    {
        _isChasing = true;
        _isWandering = false;
        _isPaused = false;
        _navMeshAgent.isStopped = false;
    }

    private void StopChasing()
    {
        _isChasing = false;
        StartWandering();
    }

    private void ChasePlayer()
    {
        if (_player != null)
        {
            _navMeshAgent.SetDestination(_player.transform.position);
        }
    }
}