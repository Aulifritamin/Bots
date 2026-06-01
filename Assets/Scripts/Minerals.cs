using System;
using UnityEngine;

public class Minerals : MonoBehaviour
{
    private const string Name = "Mineral";
    private Transform _spawnPoint;

    private Vector3 _initialScale;
    private Vector3 _inventoryScale = new Vector3(0.1f, 0.1f, 0.1f);

    private bool _isHit = false;
    private Collider _collider;

    public Transform SpawnPoint => _spawnPoint;
    public bool IsHit => _isHit;

    public string MineralName => Name;

    public event Action<Minerals> OnMineralCollected;

    public void Init(Transform spawnPoint)
    {
        _spawnPoint = spawnPoint;
        ChangePosition(_spawnPoint);
        transform.localScale = _initialScale;
        if (_collider != null)
        {
            _collider.enabled = true;
        }
    }

    public void ChangeScale()
    {
        transform.localScale = _inventoryScale;
        if (_collider != null)
        {
            _collider.enabled = false;
        }
    }

    public void Hit()
    {
        if (_isHit)
            return;

        _isHit = true;
    }

    public void Drop()
    {
        _isHit = false;

        if (_collider != null)
        {
            _collider.enabled = true; 
        }     
            
        Collected();
    }

    private void ChangePosition(Transform spawnPoint)
    {
        transform.position = spawnPoint.position;
    }

    private void Awake()
    {
        _initialScale = transform.localScale;
        _collider = GetComponent<Collider>();
    }

    private void Collected()
    {
        OnMineralCollected?.Invoke(this);
    }
}
