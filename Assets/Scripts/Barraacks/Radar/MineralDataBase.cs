using System;
using System.Collections.Generic;
using UnityEngine;

public class MineralDataBase : MonoBehaviour
{
    private HashSet<Minerals> _knownMinerals = new HashSet<Minerals>();

    public event Action<Minerals> TaskAdded;

    public void AddMineral(Minerals mineral)
    {
        AddToDataBase(mineral);
    }

    public void RemoveMineral(List<Minerals> minerals)
    {
        foreach (Minerals mineral in minerals)
        {
            RemoveFromDataBase(mineral);
        }
    }

    private void AddToDataBase(Minerals mineral)
    {
        if (_knownMinerals.Add(mineral))
        {
            TaskAdded?.Invoke(mineral);
        }
    }

    private void RemoveFromDataBase(Minerals mineral)
    {
        _knownMinerals.Remove(mineral);
    }
}
