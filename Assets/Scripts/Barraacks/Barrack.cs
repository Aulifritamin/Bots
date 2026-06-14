using UnityEngine;

public class Barrack : MonoBehaviour
{
    public int _maxFlag { get; } = 1;
    public int _currentFlag { get; private set; } = 0;

    public void Increase()
    {
        _currentFlag++;
    }

    public bool CheckAvaible()
    {
        return _currentFlag < _maxFlag;
    }

    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }
}
