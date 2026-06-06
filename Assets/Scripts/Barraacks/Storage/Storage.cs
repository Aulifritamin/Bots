using System;
using System.Collections.Generic;
using UnityEngine;

public class Storage : MonoBehaviour
{
    private Dictionary<string, int> _storage = new Dictionary<string, int>();

    public Action<Dictionary<string, int>> CountChanged;

    public void AddMinerals(List<Minerals> deliveredMinerals)
    {
        foreach (Minerals mineral in deliveredMinerals)
        {
            if (!_storage.ContainsKey(mineral.MineralName))
            {
                _storage.Add(mineral.MineralName, 1);
            }
            else
            {
                _storage[mineral.MineralName]++;
            }

            CountChanged?.Invoke(_storage);
        }
    }

    public void Decrease(string mineralName, int count)
    {
        if (_storage.ContainsKey(mineralName) && _storage[mineralName] >= count)
        {
            _storage[mineralName] -= count;
            CountChanged?.Invoke(_storage);
        }
    }
}
