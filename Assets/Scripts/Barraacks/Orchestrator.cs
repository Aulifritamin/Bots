using System;
using System.Collections.Generic;
using UnityEngine;

public class Orchestrator : MonoBehaviour
{
    [SerializeField] private Barrack _baseprefub;

    [SerializeField] private UnitSpawner _unitSpawner;
    [SerializeField] private MineralDataBase _mineralDataBase;
    [SerializeField] private MineralDetector _mineralDetector;
    [SerializeField] private VehicleManager _vehicleManager;
    [SerializeField] private Storage _storage;
    [SerializeField] private InputListener _inputListener;

    private ClickState _currentClickState = ClickState.WaitingForBase;
    private Barrack _selectedBarrack;

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
        _inputListener.MapClicked += OnClickDetected;
        _unitSpawner.AddedUnit += _vehicleManager.AddVehicle;
        _mineralDetector.MineralDetected += OnMineralDetected;
        _vehicleManager.MineralsDelivered += _storage.AddMinerals;
        _vehicleManager.MineralsDelivered += _mineralDataBase.RemoveMineral;
        _storage.CountChanged += InitNewUnit;
    }

    private void OnDisable()
    {
        _inputListener.MapClicked -= OnClickDetected;
        _unitSpawner.AddedUnit -= _vehicleManager.AddVehicle;
        _mineralDetector.MineralDetected -= OnMineralDetected;
        _vehicleManager.MineralsDelivered -= _storage.AddMinerals;
        _vehicleManager.MineralsDelivered -= _mineralDataBase.RemoveMineral;
        _storage.CountChanged -= InitNewUnit;
    }

    private void OnClickDetected(RaycastHit hit)
    {
        if (_currentClickState == ClickState.WaitingForBase)
        {
            if (hit.collider.TryGetComponent(out Barrack barrack))
            {
                Debug.Log("found collide barak");
                if (barrack.CheckAvaible())
                {
                    _selectedBarrack = barrack;
                    _currentClickState = ClickState.WaitingForGround;
                }
            }
        }

        else if (_currentClickState == ClickState.WaitingForGround)
        {
            Debug.Log("need ground");
            if (hit.collider.CompareTag("Ground"))
            {
                Vector3 targetGroundPos = hit.point;

                Instantiate(_baseprefub, targetGroundPos, Quaternion.identity);
                _selectedBarrack.Increase();

                _currentClickState = ClickState.WaitingForBase;
            }
        }
    }

    private void OnMineralDetected(Minerals mineral)
    {
        if (_mineralDataBase.AddMineral(mineral))
        {
            _vehicleManager.AddingTask(mineral);
        }
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

    private enum ClickState { WaitingForBase, WaitingForGround }
}
