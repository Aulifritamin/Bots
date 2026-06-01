using System;
using System.Collections.Generic;
using UnityEngine;

public class Storage : MonoBehaviour
{
    private List<Minerals> _storedMinerals = new List<Minerals>();

    public Action<int> OnCapacityChanged;

    private void Awake()
    {
        OnCapacityChanged?.Invoke(_storedMinerals.Count);
    }

    public void AddMinerals(List<Minerals> deliveredMinerals)
    {
        _storedMinerals.AddRange(deliveredMinerals);
        OnCapacityChanged?.Invoke(_storedMinerals.Count);
    }
}
