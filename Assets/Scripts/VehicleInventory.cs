using System.Collections.Generic;
using UnityEngine;

public class VehicleInventory : MonoBehaviour
{
    [SerializeField] private Transform _trunk;

    private int _capacity = 1;
    private int _currentLoad = 0;

    private List<Minerals> _minerals = new List<Minerals>();

    public void AddMineral(Minerals mineral)
    {
        if (_minerals.Count >= _capacity)
        {
            return;
        }

        _minerals.Add(mineral);
        mineral.transform.SetParent(_trunk, false);
        mineral.transform.localPosition = Vector3.zero;
        mineral.ChangeScale();
        _currentLoad++;
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
        List<Minerals> mineralsCopy = new List<Minerals>(_minerals);
        
        return mineralsCopy;
    }
}
