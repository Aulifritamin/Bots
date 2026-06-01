using System;
using System.Collections;
using UnityEngine;

public class MineralDetector : MonoBehaviour
{
    [SerializeField] private Transform _radarRotator;
    [SerializeField] private float _scanRange = 30f;
    [SerializeField] private float _rotationSpeed = 30f;
    [SerializeField] private float _sphereRadius = 3f;
    [SerializeField] private LayerMask _mineralLayer;

    private LineRenderer _lineRenderer;

    public event Action<Minerals> OnMineralDetected;

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.useWorldSpace = true;
    }

    private void Update()
    {
        ActivateRadar();
    }

    private void ActivateRadar()
    {
        if(_radarRotator != null)
        {
            _radarRotator.Rotate(Vector3.up, _rotationSpeed * Time.deltaTime);
        }

        Vector3 origin = _radarRotator.position;
        Vector3 direction = _radarRotator.forward;

        _lineRenderer.SetPosition(0, origin);

        RaycastHit[] hits = Physics.SphereCastAll(origin, _sphereRadius, direction, _scanRange, _mineralLayer);

        if (hits.Length > 0)
        {
            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.TryGetComponent(out Minerals mineral))
                {
                    if (mineral.IsHit) 
                    {
                        continue; 
                    }

                    mineral.Hit();
                    OnMineralDetected?.Invoke(mineral);
                }
            }
        }

        _lineRenderer.SetPosition(1, origin + direction * _scanRange);
    }

    private void OnDrawGizmos()
    {
        if (_radarRotator != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(_radarRotator.position, _sphereRadius);
            Gizmos.DrawLine(_radarRotator.position, _radarRotator.position + _radarRotator.forward * _scanRange);
        }
    }
}
