using System;
using System.Collections.Generic;
using UnityEngine;

public class WorkerManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private WorkerSystemData workerSystemData;
    [SerializeField] private WorkerUnit workerPrefab;
    [SerializeField] private OreGridCoordinateProvider gridProvider;
    [SerializeField] private OreSpawnController oreSpawnController;
    [SerializeField] private WarehouseInventory warehouseInventory;

    [SerializeField] private ParticleSystem workerPart;

    [Header("Runtime")]
    [SerializeField] private bool hasHiredDefaultWorkers;
    [SerializeField] private List<WorkerUnit> spawnedWorkers = new List<WorkerUnit>();

    public event Action OnDefaultWorkersHired;
    public bool HasHiredDefaultWorkers => hasHiredDefaultWorkers;

    public bool HireDefaultWorkers()
    {
        if (hasHiredDefaultWorkers)
        {
            return false;
        }

        if (workerSystemData == null ||
            workerPrefab == null ||
            gridProvider == null ||
            oreSpawnController == null ||
            warehouseInventory == null)
        {
            return false;
        }

        List<int> rows = workerSystemData.defaultAssignedRows;

        for (int i = 0; i < rows.Count; i++)
        {
            SpawnWorker(rows[i]);
        }

        hasHiredDefaultWorkers = true;

        OnDefaultWorkersHired?.Invoke();

        return true;
    }

    public WorkerUnit SpawnWorker(int row)
    {
        workerPart.Play();
        WorkerUnit workerUnit = Instantiate(workerPrefab, transform);
        workerUnit.Initialize(row, workerSystemData, gridProvider, oreSpawnController, warehouseInventory);

        spawnedWorkers.Add(workerUnit);

        return workerUnit;
    }
}