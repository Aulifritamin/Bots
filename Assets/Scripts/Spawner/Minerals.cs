using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Minerals : MonoBehaviour
{
    private const string Name = "Mineral";
    private Transform _spawnPoint;

    private Vector3 _initialScale;

    private Collider _collider;

    public Transform SpawnPoint => _spawnPoint;

    public string MineralName => Name;

    public event Action<Minerals> MineralCollected;

    private void Awake()
    {
        _initialScale = transform.localScale;
        _collider = GetComponent<Collider>();
    }

    public void Init(Transform spawnPoint)
    {
        _spawnPoint = spawnPoint;
        transform.position = _spawnPoint.position;
        transform.localScale = _initialScale;
        _collider.enabled = true;
    }

    public void PickedUp()
    {
        _collider.enabled = false;
    }

    public void Drop()
    {
        _collider.enabled = true;
        MineralCollected?.Invoke(this);
    }
}
