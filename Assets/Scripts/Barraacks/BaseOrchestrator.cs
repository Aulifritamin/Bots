using System;
using System.Collections.Generic;
using UnityEngine;

public class BaseOrchestrator : MonoBehaviour
{
    [SerializeField] private Barrack _basePrefub;
    [SerializeField] private FlagManager _flagManager;
    [SerializeField] private UnitSpawner _unitSpawner;
    [SerializeField] private MineralDataBase _mineralDataBase;
    [SerializeField] private MineralDetector _mineralDetector;
    [SerializeField] private VehicleManager _vehicleManager;
    [SerializeField] private Storage _storage;

    private BaseSetting _currentBaseSetting = BaseSetting.Idle;
    private int _barrackCost = 5;
    private int _minUnitForNewBase = 2;

    private Vector3 _flagPosition;

    public bool IsAvailable()
    {
        return _flagManager.IsAvailable() && _unitSpawner.CurrentUnitCount >= _minUnitForNewBase;
    }

    public void ActivateBuildingBarrack(Vector3 flagPosition)
    {
        _flagPosition = flagPosition;
        _currentBaseSetting = BaseSetting.BuildingBarrack;
    }

    private void Awake()
    {
        _unitSpawner = GetComponent<UnitSpawner>();
        _mineralDetector = GetComponent<MineralDetector>();
        _vehicleManager = GetComponent<VehicleManager>();
        _storage = GetComponent<Storage>();
    }

    private void OnEnable()
    {
        _unitSpawner.AddedUnit += _vehicleManager.AddVehicle;
        _mineralDetector.MineralDetected += OnMineralDetected;
        _vehicleManager.MineralsDelivered += _storage.AddMinerals;
        _vehicleManager.MineralsDelivered += _mineralDataBase.RemoveMineral;
        _vehicleManager.BuildingVehicleAvailable += StartBuildingBarrack;
        _storage.CountChanged += CheckSetting;
    }

    private void OnDisable()
    {
        _unitSpawner.AddedUnit -= _vehicleManager.AddVehicle;
        _mineralDetector.MineralDetected -= OnMineralDetected;
        _vehicleManager.MineralsDelivered -= _storage.AddMinerals;
        _vehicleManager.MineralsDelivered -= _mineralDataBase.RemoveMineral;
        _vehicleManager.BuildingVehicleAvailable -= StartBuildingBarrack;
        _storage.CountChanged -= CheckSetting;
    }

    private void OnMineralDetected(Minerals mineral)
    {
        if (_mineralDataBase.AddMineral(mineral))
        {
            _vehicleManager.AddingTask(mineral);
        }
    }

    private void CheckSetting(IReadOnlyDictionary<string, int> storage)
    {
        if (_currentBaseSetting == BaseSetting.BuildingBarrack)
        {
            if(_storage.GetMineralCount() >= _barrackCost)
            {
                _vehicleManager.NeedToBuildVehicle();
            }

        }
        else if (_currentBaseSetting == BaseSetting.Idle)
        {
            InitNewUnit(storage);
        }
    }

    private void StartBuildingBarrack(Vehicle vehicle)
    {
        _flagManager.Increase();
        _unitSpawner.DecreaseUnitCount();
        _currentBaseSetting = BaseSetting.Idle;

        vehicle.GoToBuild(_flagPosition);
        _storage.Decrease("Mineral", _barrackCost);
        vehicle.ArrivedAtBuildSite += OnBuilderArrived;
    }

    private void OnBuilderArrived(Vehicle builder)
    {
        builder.ArrivedAtBuildSite -= OnBuilderArrived; 

        Barrack newBarrack = Instantiate(_basePrefub, _flagPosition, Quaternion.identity);
        Transform newBaseReturnPoint = newBarrack.GetUnitReturnTransform();
        
        builder.CompleteBuilding(newBaseReturnPoint.position);
        
        newBarrack.SetActive();
        newBarrack.AddUnitToNewBase(builder);
    }

    private void InitNewUnit(IReadOnlyDictionary<string, int> storage)
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

    private enum BaseSetting { Idle, BuildingBarrack }
}
