using UnityEngine;

public class FlagManager : MonoBehaviour
{
    [SerializeField] private Flag _flagPrefab;

    private int _maxFlag = 1;
    private int _currentFlag = 0;

    public void Increase()
    {
        _currentFlag++;
    }

    public bool IsAvailable()
    {
        return _currentFlag < _maxFlag;
    }

    public void Activate(Vector3 position)
    {
        _flagPrefab.SetPosition(position);
        _flagPrefab.ActivateFlag();
    }

    public void Deactivate()
    {
        _flagPrefab.ResetFlag();
    }
}
