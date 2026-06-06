using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(VehicleInventory), typeof(VehicleMovement))]
public class Vehicle : MonoBehaviour
{
    [SerializeField] private VehicleInventory _inventory;
    [SerializeField] private VehicleMovement _movement;

    private Minerals _targetMineral;
    private Vector3 _targetPosition;
    private Vector3 _basePosition;
    private Vector3 _spawnPosition;

    public Action<Vehicle> VehicleAvailable;
    public Action<List<Minerals>> MineralDelivered;

    private void Start()
    {
        _spawnPosition = transform.position;
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
            Release();
            Activate(_spawnPosition);
            VehicleAvailable?.Invoke(this);
        }
    }

    public void SetTarget(Minerals targetMineral)
    {
        _targetMineral = targetMineral;
        _targetPosition = targetMineral.transform.position;
        Activate(_targetPosition);
    }

    public void SetReturnPos(Transform spawnPoint)
    {
        _basePosition = spawnPoint.position;
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
}
