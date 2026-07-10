using UnityEngine;

public class Barrack : MonoBehaviour
{
    [SerializeField] private UnitSpawner _unitSpawner;

    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }

    public void SetActive()
    {
        gameObject.SetActive(true);
    }

    public void SetInactive()
    {
        gameObject.SetActive(false);
    }

    public Transform GetUnitReturnTransform()
    {
        return _unitSpawner.GetReturnTransform();
    }

    public void AddUnitToNewBase(Vehicle unit)
    {
        _unitSpawner.IsNewBase();
        _unitSpawner.AddExistingUnit(unit);
    }
}
