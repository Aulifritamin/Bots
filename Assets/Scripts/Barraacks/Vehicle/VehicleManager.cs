using System;
using System.Collections.Generic;
using UnityEngine;

public class VehicleManager : MonoBehaviour
{
    private Queue<Vehicle> _activeUnits = new Queue<Vehicle>();
    private Queue<Minerals> _pendingTasks = new Queue<Minerals>();

    public Action<List<Minerals>> MineralsDelivered;

    public void AddingTask(Minerals mineral)
    {
        _pendingTasks.Enqueue(mineral);
        TryActivateVehicle();
    }

    public void AddVehicle(Vehicle vehicle)
    {
        _activeUnits.Enqueue(vehicle);
        TryActivateVehicle();
    }

    private void TryActivateVehicle()
    {
        if (_activeUnits.Count == 0)
        {
            return;
        }

        if (_pendingTasks.Count == 0)
        {
            return;
        }

        Vehicle vehicle = _activeUnits.Dequeue();
        Minerals mineral = _pendingTasks.Dequeue();

        vehicle.VehicleAvailable += OnVehicleAvailable;
        vehicle.MineralDelivered += OnMineralDelivered;
        vehicle.SetTarget(mineral);
    }

    private void OnMineralDelivered(List<Minerals> minerals)
    {
        MineralsDelivered?.Invoke(minerals);
    }

    private void OnVehicleAvailable(Vehicle vehicle)
    {
        vehicle.VehicleAvailable -= OnVehicleAvailable;
        vehicle.MineralDelivered -= OnMineralDelivered;
        _activeUnits.Enqueue(vehicle);

        TryActivateVehicle();
    }
}
