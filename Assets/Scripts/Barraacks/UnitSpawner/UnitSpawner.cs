using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class UnitSpawner : MonoBehaviour
{
    [SerializeField] private Vehicle _unitPrefab;
    [SerializeField] private int _unitCount = 3;
    [SerializeField] private int _maxUnitCount = 5;

    [SerializeField] private int _costUnit = 3;

    [SerializeField] private Transform _returnPos;
    [SerializeField] private Transform _pointsContainer;
    [SerializeField] private Transform[] _spawnPoints;
    private int _spawnIndex = 0;

    private ObjectPool<Vehicle> _unitGrabberPool;

    public int MaxUnitCount => _maxUnitCount;
    public int CurrentUnitCount => _unitCount;
    public int CostUnit => _costUnit;

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

    private void InitUnits()
    {
        for (int i = 0; i < _unitCount; i++)
        {
            Vehicle vehicle = _unitGrabberPool.Get();
            vehicle.SetReturnPos(_returnPos);
            vehicle.transform.position = _spawnPoints[_spawnIndex % _spawnPoints.Length].position;
            _spawnIndex++;
            vehicle.gameObject.SetActive(true);
            AddedUnit?.Invoke(vehicle);
        }
    }

    private void AddNewUnit()
    {
        Debug.Log($"{_unitCount}");

        if (_unitCount >= _maxUnitCount)
        {
            return;
        }

        Vehicle vehicle = _unitGrabberPool.Get();
        vehicle.SetReturnPos(_returnPos);
        vehicle.transform.position = _spawnPoints[_spawnIndex % _spawnPoints.Length].position;
        _spawnIndex++;
        vehicle.gameObject.SetActive(true);
        _unitCount++;
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
