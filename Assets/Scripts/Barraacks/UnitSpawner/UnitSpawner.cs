using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class UnitSpawner : MonoBehaviour
{
    [SerializeField] private Vehicle _unitPrefab;
    [SerializeField] private int _InitUnitCount = 3;
    [SerializeField] private int _maxUnitCount = 5;

    [SerializeField] private int _costUnit = 3;

    [SerializeField] private Transform _returnPos;
    [SerializeField] private Transform _pointsContainer;
    [SerializeField] private Transform[] _spawnPoints;
    private int _spawnIndex = 0;

    private ObjectPool<Vehicle> _unitGrabberPool;

    public int MaxUnitCount => _maxUnitCount;
    public int CostUnit => _costUnit;
    public int CurrentUnitCount  {get; private set;} = 0;

    public event Action<Vehicle> AddedUnit;

    private void Awake()
    {
        _unitGrabberPool = new ObjectPool<Vehicle>(
            createFunc: () => Instantiate(_unitPrefab),
            actionOnGet: unit => unit.gameObject.SetActive(false),
            actionOnRelease: unit => unit.gameObject.SetActive(false),
            actionOnDestroy: unit => Destroy(unit.gameObject),
            collectionCheck: false,
            defaultCapacity: _maxUnitCount,
            maxSize: _maxUnitCount
        );

        RefreshChildArray();
    }

    private void Start()
    {
        InitUnits();
    }

    public void InitNewUnit()
    {
        AddNewUnit();
    }
    
    public void DecreaseUnitCount()
    {
        if (CurrentUnitCount > 0)
        {
            CurrentUnitCount--;
        }
    }

    public void IsNewBase()
    {
        _InitUnitCount = 0;
    }

    public Transform GetReturnTransform()
    {
        return _returnPos;
    }

    public void AddExistingUnit(Vehicle vehicle)
    {
        vehicle.SetUnitSpawner(this);
        vehicle.SetSpawnPosition(_spawnPoints[_spawnIndex % _spawnPoints.Length].position);
        vehicle.SetReturnBasePosition(_returnPos);
        _spawnIndex++;
        AddedUnit?.Invoke(vehicle);
        CurrentUnitCount++;
    }

    private void InitUnits()
    {
        for (int i = 0; i < _InitUnitCount; i++)
        {
            AddNewUnit();
        }
    }

    private void AddNewUnit()
    {
        if (CurrentUnitCount >= _maxUnitCount)
        {
            return;
        }

        Vehicle vehicle = _unitGrabberPool.Get();
        vehicle.SetUnitSpawner(this);
        vehicle.SetReturnBasePosition(_returnPos);
        vehicle.SetSpawnPosition(_spawnPoints[_spawnIndex % _spawnPoints.Length].position);
        _spawnIndex++;
        vehicle.gameObject.SetActive(true);
        CurrentUnitCount++;
        AddedUnit?.Invoke(vehicle);
    }

    [ContextMenu("Refresh Child Array")]
    private void RefreshChildArray()
    {
        int pointCount = _pointsContainer.childCount;
        _spawnPoints = new Transform[pointCount];

        for (int i = 0; i < pointCount; i++)
        {
            _spawnPoints[i] = _pointsContainer.GetChild(i);
        }
    }
}
