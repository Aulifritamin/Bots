using System;
using System.Collections.Generic;
using UnityEngine;

public class MineralDataBase : MonoBehaviour
{
    private HashSet<Minerals> _knownMinerals = new HashSet<Minerals>();

    public bool AddMineral(Minerals mineral)
    {
        if (_knownMinerals.Add(mineral))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void RemoveMineral(List<Minerals> minerals)
    {
        foreach (Minerals mineral in minerals)
        {
            RemoveFromDataBase(mineral);
        }
    }

    private void RemoveFromDataBase(Minerals mineral)
    {
        _knownMinerals.Remove(mineral);
    }
}
