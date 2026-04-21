using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WorkerSystemData", menuName = "Game/Worker System Data")]
public class WorkerSystemData : ScriptableObject
{
    [Header("Hire")]
    public int hireCost = 50;

    [Header("Worker Movement")]
    public float moveSpeed = 1f;
    public float workDelay = 1f;

    [Header("Assigned Rows")]
    public List<int> defaultAssignedRows = new List<int> { 5, 6, 7 };
}
