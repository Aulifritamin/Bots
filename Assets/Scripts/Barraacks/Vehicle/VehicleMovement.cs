using System;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class VehicleMovement : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _agent;
    private Vector3 _targetPosition;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    public void Activate(Vector3 target)
    {
        _targetPosition = target;
        Move();
    }

    private void Move()
    {
        if (_targetPosition != null)
        {
            _agent.SetDestination(_targetPosition);
        }
    }
}
