using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class UnitSpawner : MonoBehaviour
{
    [SerializeField] private Vehicle _unitPrefab;
    [SerializeField] private int _unitCount = 3;
    [SerializeField] private int _maxUnitCount = 10;

    [SerializeField] private Transform _returnPos;
    [SerializeField] private Transform _pointsContainer;
    [SerializeField] private Transform[] _spawnPoints;

    private ObjectPool<Vehicle> _unitGrabberPool;
    private Queue<Vehicle> _activeUnits = new Queue<Vehicle>();

    public event Action<List<Minerals>> OnMineralsDelivered;


    private void Awake()
    {
        _unitGrabberPool = new ObjectPool<Vehicle>(
            createFunc: () => Instantiate(_unitPrefab),
            actionOnGet: unit => unit.gameObject.SetActive(false),
            actionOnRelease: unit => unit.gameObject.SetActive(false),
            actionOnDestroy: unit => Destroy(unit.gameObject),
            collectionCheck: false,
            defaultCapacity: _unitCount,
            maxSize: _maxUnitCount
        );

        RefreshChildArray();
    }

    private void OnEnable()
    {
        _unitPrefab.OnVehicleAvailable += ReturnUnitToPool;
    }

    private void OnDisable()
    {
        _unitPrefab.OnVehicleAvailable -= ReturnUnitToPool;
    }

    private void Start()
    {
        InitUnits();
    }

    private void ReturnUnitToPool(Vehicle vehicle)
    {
        _activeUnits.Enqueue(vehicle);
        vehicle.OnVehicleAvailable -= ReturnUnitToPool;
    }

    private void SetDestination(Minerals target)
    {
        if (_activeUnits.Count == 0)
        {
            return;
        }

        Vehicle vehicle = _activeUnits.Dequeue();

        vehicle.OnVehicleAvailable += ReturnUnitToPool;
        vehicle.SetTarget(target.transform);
    }

    private void InitUnits()
    {
        for (int i = 0; i < _unitCount; i++)
        {
            Vehicle vehicle = _unitGrabberPool.Get();
            vehicle.SetReturnPos(_returnPos);
            vehicle.transform.position = _spawnPoints[i].position;
            vehicle.gameObject.SetActive(true);
            vehicle.OnMineralsDelivered += HandleMineralsDelivered;
            _activeUnits.Enqueue(vehicle);
        }
    }

    private void HandleMineralsDelivered(List<Minerals> deliveredMinerals)
    {
        OnMineralsDelivered?.Invoke(deliveredMinerals);
    }

    [ContextMenu("Refresh Child Array")]
    private void RefreshChildArray()
    {
        int pointCount = _pointsContainer.childCount;
        _spawnPoints = new Transform[pointCount];

        for (int i = 0; i < pointCount; i++)
            _spawnPoints[i] = _pointsContainer.GetChild(i);
    }

    public void MoveToMineral(Minerals target)
    {
        SetDestination(target);
    }
}
