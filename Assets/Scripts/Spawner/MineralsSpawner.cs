using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class MineralsSpawner : MonoBehaviour
{
    [SerializeField] private Minerals _mineralPrefub;

    [SerializeField] private float _spawnInterval = 10f;

    [SerializeField] private Transform[] _spawnPoints;
    [SerializeField] private Transform _pointsContainer;

    private List<Transform> _freePoints = new List<Transform>();
    private List<Transform> _occupiedPoints = new List<Transform>();

    private ObjectPool<Minerals> _mineralsPool;
    

    private void Awake()
    {
        _mineralsPool = new ObjectPool<Minerals>(
            createFunc: () => Instantiate(_mineralPrefub),
            actionOnGet: GettingFromPool,
            actionOnRelease: ReleasingToPool,
            actionOnDestroy: mineral => Destroy(mineral.gameObject),
            collectionCheck: false,
            defaultCapacity: _spawnPoints.Length,
            maxSize: 10
        );

        RefreshChildArray();
        _freePoints.AddRange(_spawnPoints);
    }

    private void Start()
    {
        StartCoroutine(Activate());
    }

    private IEnumerator Activate()
    {
        WaitForSeconds wait = new WaitForSeconds(_spawnInterval);

        while (enabled)
        {
            yield return wait;

            if (_freePoints.Count == 0)
                {
                    continue;
                }

            _mineralsPool.Get();
        }
    }

    private void GettingFromPool(Minerals mineral)
    {
        Transform spawnPoint = GetRandomPosition();
        mineral.gameObject.SetActive(true);
        mineral.Init(spawnPoint);
        mineral.MineralCollected += ReturningMineralToPool;
    }

    private void ReleasingToPool(Minerals mineral)
    {
        mineral.MineralCollected -= ReturningMineralToPool;
        mineral.gameObject.SetActive(false);
    }

    private void ReturningMineralToPool(Minerals mineral)
    {
        if(_occupiedPoints.Contains(mineral.SpawnPoint))
        {
            _occupiedPoints.Remove(mineral.SpawnPoint);
            _freePoints.Add(mineral.SpawnPoint);

            if (mineral.transform.TryGetComponent(out SpawnPoint point))
            {
                point.ToggleAvailable();
            }
        }
            
        _mineralsPool.Release(mineral);
    }

    private Transform GetRandomPosition()
    {
        int minIndex = 0;
        int maxIndex = _freePoints.Count;

        int randomIndex = Random.Range(minIndex, maxIndex);
        Transform spawnPoint = _freePoints[randomIndex];

        _occupiedPoints.Add(spawnPoint);
        _freePoints.Remove(spawnPoint);

        if(spawnPoint.TryGetComponent(out SpawnPoint point))
        {
            point.ToggleUnavailable();
        }

        return spawnPoint;
    }

    [ContextMenu("Refresh Child Array")]
    private void RefreshChildArray()
    {
        int pointCount = _pointsContainer.childCount;
        _spawnPoints = new Transform[pointCount];

        for (int i = 0; i < pointCount; i++)
            _spawnPoints[i] = _pointsContainer.GetChild(i);
    }
}
