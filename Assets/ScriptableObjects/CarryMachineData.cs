using UnityEngine;

[CreateAssetMenu(fileName = "CarryMachineData", menuName = "Game/Carry Machine Data")]
public class CarryMachineData : ScriptableObject
{
    [Header("Purchase")]
    public int purchaseCost = 50;

    [Header("Movement")]
    public float moveSpeed = 2f;

    [Header("Carry")]
    public int carryAmount = 5;

    [Header("Waypoints")]
    public Vector3 point1 = new Vector3(-7.82999992f, 0.50999999f, -0.216999993f);
    public Vector3 point2 = new Vector3(-6.57999992f, 0.50999999f, -5.09000015f);

    [Header("Spawn")]
    public Vector3 spawnPosition = new Vector3(-6.57999992f, 0.50999999f, -5.09000015f);
}