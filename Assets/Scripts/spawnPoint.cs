using UnityEngine;

public class spawnPoint : MonoBehaviour
{
    private bool _isAvaible = true;

    public bool IsActibe => _isAvaible;
    public Transform Position => transform;

    public void ChangeAvaible()
    {
        _isAvaible = !_isAvaible;
    }
}
