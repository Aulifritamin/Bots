using System;
using System.Collections;
using UnityEngine;

public class MineralDetector : MonoBehaviour
{
    [SerializeField] private Vector3 _radar;
    [SerializeField] private float _scanRange = 40f;
    [SerializeField] private float _checkInterval = 5f;
    [SerializeField] private LayerMask _mineralLayer;

    private Collider[] _hitResults = new Collider[20];

    public event Action<Minerals> MineralDetected;

    private void Awake()
    {
        _radar = transform.position;
    }

    private void Start()
    {
        StartCoroutine(ActivateRadar());
    }

    private IEnumerator ActivateRadar()
    {
        WaitForSeconds wait = new WaitForSeconds(_checkInterval);

        while (enabled)
        {
            yield return wait;

            int hitsCount = Physics.OverlapSphereNonAlloc(_radar, _scanRange, _hitResults, _mineralLayer);

            for (int i = 0; i < hitsCount; i++)
            {
                Collider hit = _hitResults[i];

                if (hit.TryGetComponent(out Minerals mineral))
                {
                    MineralDetected?.Invoke(mineral);
                }
            }
        }
    }
}
