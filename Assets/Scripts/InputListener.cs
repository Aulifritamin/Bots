using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class InputListener : MonoBehaviour
{
    private PlayerInput _playerInput;
    private InputAction _clickAction;
    private InputAction _positionAction;

    public event Action<RaycastHit> MapClicked;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();

        _clickAction = _playerInput.actions.FindActionMap("Player").FindAction("Attack");
        _positionAction = _playerInput.actions.FindActionMap("Player").FindAction("Position");
    }

    private void OnEnable()
    {
        _clickAction.started += OnClick;
    }

    private void OnDisable()
    {
        _clickAction.started -= OnClick;
    }

    private void OnClick(InputAction.CallbackContext context)
    {
        Vector2 screenPosition = _positionAction.ReadValue<Vector2>();

        Ray ray = Camera.main.ScreenPointToRay(screenPosition);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
        {
            MapClicked?.Invoke(hit);
        }
    }
}
