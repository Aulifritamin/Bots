using System.Collections.Generic;
using UnityEngine;

public class Orchestrator : MonoBehaviour
{
    [SerializeField] private UnitSpawner _unitSpawner;
    [SerializeField] private MineralDataBase _mineralDataBase;
    [SerializeField] private MineralDetector _mineralDetector;
    [SerializeField] private VehicleManager _vehicleManager;
    [SerializeField] private Storage _storage;

    private void Awake()
    {
        _unitSpawner = GetComponent<UnitSpawner>();
        _mineralDataBase = GetComponent<MineralDataBase>();
        _mineralDetector = GetComponent<MineralDetector>();
        _vehicleManager = GetComponent<VehicleManager>();
        _storage = GetComponent<Storage>();
    }

    private void OnEnable()
    {
        _unitSpawner.AddedUnit += _vehicleManager.AddVehicle;
        _mineralDetector.MineralDetected += _mineralDataBase.AddMineral;
        _mineralDataBase.TaskAdded += _vehicleManager.AddingTask;
        _vehicleManager.MineralsDelivered += _storage.AddMinerals;
        _vehicleManager.MineralsDelivered += _mineralDataBase.RemoveMineral;
        _storage.CountChanged += InitNewUnit;
    }

    private void OnDisable()
    {
        _unitSpawner.AddedUnit -= _vehicleManager.AddVehicle;
        _mineralDetector.MineralDetected -= _mineralDataBase.AddMineral;
        _mineralDataBase.TaskAdded -= _vehicleManager.AddingTask;
        _vehicleManager.MineralsDelivered -= _storage.AddMinerals;
        _vehicleManager.MineralsDelivered -= _mineralDataBase.RemoveMineral;
        _storage.CountChanged -= InitNewUnit;
    }

    private void InitNewUnit(Dictionary<string, int> storage)
    {
        foreach (var amount in storage)
        {
            if (amount.Value >= _unitSpawner.CostUnit && _unitSpawner.CurrentUnitCount < _unitSpawner.MaxUnitCount)
            {
                _unitSpawner.InitNewUnit();
                _storage.Decrease(amount.Key, _unitSpawner.CostUnit);
                break;
            }
        }
    }
}
