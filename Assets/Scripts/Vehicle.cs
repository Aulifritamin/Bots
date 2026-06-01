using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Vehicle : MonoBehaviour
{
    [SerializeField] private VehicleInventory _inventory;

    private Vector3 _targetPosition;
    private Vector3 _basePosition;
    private Vector3 _spawnPosition;
    private NavMeshAgent _agent;

    public Action<Vehicle> OnVehicleAvailable;
    public Action<List<Minerals>> OnMineralsDelivered;

    public void SetTarget(Transform target)
    {
        _targetPosition = target.position;
        Activate(_targetPosition);
    }

    public void SetReturnPos(Transform spawnPoint)
    {
        _basePosition = spawnPoint.position;
    }

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        _spawnPosition = transform.position;
    }

    private void Activate(Vector3 target)
    {
        _agent.SetDestination(target);
    }
   
    private void Grab(Minerals mineral)
    {
        _inventory.AddMineral(mineral);
    }

    private void Release()
    {
        OnMineralsDelivered?.Invoke(_inventory.GetMinerals());
        _inventory.DropMinerals();
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.TryGetComponent(out Minerals mineral))
        {
            Grab(mineral);
            Activate(_basePosition);
        }

        if (collider.TryGetComponent(out UnitSpawner spawner))
        {
            Release();
            Activate(_spawnPosition);
            OnVehicleAvailable?.Invoke(this);
        }
    }
}
