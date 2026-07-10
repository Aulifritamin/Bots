    using UnityEngine;

    public class GameManager : MonoBehaviour
    {
    [SerializeField] private InputListener _inputListener;
    [SerializeField] private Flag _flagPrefab;

    private string _groundLayerMask = "Ground";

    private ClickState _currentClickState = ClickState.WaitingForBase;
    private BaseOrchestrator _selectedBase;

    private void Awake()
    {
        _inputListener = GetComponent<InputListener>();
    }
    
    private void OnEnable()
    {
        _inputListener.MapClicked += OnClickDetected;
    }

    private void OnDisable()
    {
        _inputListener.MapClicked -= OnClickDetected;
    }

    private void OnClickDetected(RaycastHit hit)
    {
        if (_currentClickState == ClickState.WaitingForBase)
        {
            if (hit.collider.TryGetComponent(out BaseOrchestrator baseOrchestrator))
            {
                _selectedBase = baseOrchestrator;

                if(baseOrchestrator.IsAvailable())
                {
                    _currentClickState = ClickState.WaitingForGround;
                }
            }
        }
        else if (_currentClickState == ClickState.WaitingForGround)
        {
            if (hit.collider.CompareTag(_groundLayerMask))
            {
                Vector3 flagPosition = hit.point;

                _flagPrefab.SetPosition(flagPosition);
                _selectedBase.ActivateBuildingBarrack(flagPosition);
                _currentClickState = ClickState.WaitingForBase;
            }
        }
    }

    private enum ClickState
    {
    WaitingForBase,
    WaitingForGround,
    }

    } 
