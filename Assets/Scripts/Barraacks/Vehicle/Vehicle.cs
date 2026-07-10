using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(VehicleInventory), typeof(VehicleMovement))]
public class Vehicle : MonoBehaviour
{
    [SerializeField] private VehicleInventory _inventory;
    [SerializeField] private VehicleMovement _movement;

    private bool _isBuilder = false;

    private UnitSpawner _unitSpawner;
    private Minerals _targetMineral;
    private Vector3 _targetPosition;
    private Vector3 _basePosition;
    private Vector3 _spawnPosition;

    public event Action<Vehicle> ArrivedAtBuildSite;
    public Action<Vehicle> VehicleAvailable;
    public Action<List<Minerals>> MineralDelivered;

    private void Start()
    {
        transform.position = _spawnPosition;
    }

    public void SetTarget(Minerals targetMineral)
    {
        _targetMineral = targetMineral;
        _targetPosition = targetMineral.transform.position;
        Activate(_targetPosition);
    }

    public void SetSpawnPosition(Vector3 spawnPosition)
    {
        _spawnPosition = spawnPosition;
    }

    public void SetUnitSpawner(UnitSpawner unitSpawner)
    {
        _unitSpawner = unitSpawner;
    }

    public void GoToBuild(Vector3 targetPosition)
    {
        _isBuilder = true;
        _targetPosition = targetPosition;
        Activate(_targetPosition);
    }

    public void CompleteBuilding(Vector3 newBasePosition)
    {
        _isBuilder = false;
        _basePosition = newBasePosition;
        Activate(newBasePosition);
    }

    public void SetReturnBasePosition(Transform baseTransform)
    {
        _basePosition = baseTransform.position;
    }

    private void Activate(Vector3 target)
    {
        _movement.Activate(target);
    }

    private bool Grab(Minerals mineral)
    {
        return _inventory.AddMineral(mineral);
    }

    private void Release()
    {
        MineralDelivered?.Invoke(_inventory.GetMinerals());
        _inventory.DropMinerals();
    }

      private void OnTriggerEnter(Collider collider)
    {
        if (collider.TryGetComponent(out Minerals mineral))
        {
            if (mineral == _targetMineral)
            {
                if (Grab(mineral))
                {
                    Activate(_basePosition);
                }
            }
        }

        if (collider.TryGetComponent(out UnitSpawner spawner))
        {
            if (spawner == _unitSpawner)
            {
                Release();
                Activate(_spawnPosition);
                VehicleAvailable?.Invoke(this);
            }
        }

        if(collider.TryGetComponent(out Flag flag))
        {
            if (_isBuilder)
            {
                ArrivedAtBuildSite?.Invoke(this);
            }
        }
    }
}
