using UnityEngine;

public class Orchestrator : MonoBehaviour
{
    [SerializeField] private UnitSpawner _unitSpawner;
    [SerializeField] private MineralDetector _mineralDetector;
    [SerializeField] private Storage _storage;

    private void OnEnable()
    {
        _mineralDetector.OnMineralDetected += _unitSpawner.MoveToMineral;
        _unitSpawner.OnMineralsDelivered += _storage.AddMinerals;

    }

    private void OnDisable()
    {
        _mineralDetector.OnMineralDetected -= _unitSpawner.MoveToMineral;
        _unitSpawner.OnMineralsDelivered -= _storage.AddMinerals;
    }

    private void Awake()
    {
        _unitSpawner = GetComponent<UnitSpawner>();
        _mineralDetector = GetComponent<MineralDetector>();
    }
}
