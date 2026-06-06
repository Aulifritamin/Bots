using System.Collections.Generic;
using UnityEngine;

public class VehicleInventory : MonoBehaviour
{
    [SerializeField] private Transform _trunk;

    private int _capacity = 1;
    private int _currentLoad = 0;

    private Vector3 _inventoryScale = new Vector3(0.1f, 0.1f, 0.1f);

    private List<Minerals> _minerals = new List<Minerals>();

    public bool AddMineral(Minerals mineral)
    {
        if (_minerals.Count >= _capacity)
        {
            return false;
        }

        _minerals.Add(mineral);
        mineral.transform.SetParent(_trunk, false);
        mineral.transform.localPosition = Vector3.zero;
        mineral.transform.localScale = _inventoryScale;
        _currentLoad++;
        return true;
    }

    public void DropMinerals()
    {
        foreach (var mineral in _minerals)
        {
            mineral.transform.SetParent(null);
            mineral.Drop();
        }

        _minerals.Clear();
        _currentLoad--;
    }

    public List<Minerals> GetMinerals()
    {
        return new List<Minerals>(_minerals);
    }
}
