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
        if(_isFrozen)
        {
            if(Time.time >= _freezeEndTime)
            {
                Unfreeze();
            }

            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, _player.transform.position);

        // If player is not hiding and is within chase range, start chasing
        if (distanceToPlayer <= _chaseRange && !_playerScript.IsHiding())
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

    public void Freeze(float freezeDuration)
    {
        if(!_isFrozen)
        {
            _isFrozen = true;
            _freezeEndTime = Time.time + freezeDuration;
            _navMeshAgent.isStopped = true;
        }
    }

    private void Unfreeze()
    {
        _isFrozen = false;
        _navMeshAgent.isStopped = false;
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