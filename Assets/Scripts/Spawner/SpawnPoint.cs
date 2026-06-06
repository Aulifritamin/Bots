using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    private bool _isAvailable = true;

    public bool IsActive => _isAvailable;
    public Transform Transform => transform;

    public void ToggleAvailable()
    {
        _isAvailable = true;
    }

    public void ToggleUnavailable()
    {
        _isAvailable = false;
    }
}
