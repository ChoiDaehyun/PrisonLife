using UnityEngine;

[CreateAssetMenu(
    fileName = "EquipmentData",
    menuName = "Game/Equipment Data"
)]
public class EquipmentData : ScriptableObject
{
    [Header("Info")]
    public string equipmentId = "drill";
    public string displayName = "Drill";
    public int cost = 20;

    [Header("Mining Override")]
    public bool overrideMiningInterval = true;
    public float miningInterval = 0f;

    public bool overrideMiningRadius = true;
    public float miningRadius = 1f;

    [Header("Mining Point Override")]
    public bool overrideMiningPointCount = false;
    public int miningPointCount = 1;

    public bool overrideMiningPointSpacing = false;
    public float miningPointSpacing = 0.68f;
}
