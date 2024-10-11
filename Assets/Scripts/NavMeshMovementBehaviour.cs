using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NavMeshMovementBehaviour : MovementBehaviour
{
    private NavMeshAgent _navMeshAgent;
    private Vector3 _previousTargetPosition = Vector3.zero;

    private Vector3 _wanderTarget;

    private float _wanderTimer;
    private float _wanderDuration = 5.0f;
    private float _wanderCooldown = 3.0f;

    private bool _isWandering = false;
    private bool _isPaused = false;

    protected override void Awake()
    {
        base.Awake();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.speed = _movementSpeed;

        StartWandering();
    }

    const float MOVEMENT_EPSILON = 0.25f;

    protected override void FixedUpdate()
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
}