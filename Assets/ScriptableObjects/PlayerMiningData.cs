using UnityEngine;

[CreateAssetMenu(
    fileName = "PlayerMiningData",
    menuName = "Game/Player Mining Data"
)]
public class PlayerMiningData : ScriptableObject
{
    [Header("Mining")]
    public float miningInterval = 1f;
    public float miningRadius = 0.45f;

    [Header("Respawn")]
    public float oreRespawnDelay = 2f;

    [Header("Physics")]
    public LayerMask oreLayerMask;
    public int overlapBufferSize = 16;

    [Header("Default Mining Points")]
    public int defaultMiningPointCount = 1;
    public float defaultMiningPointSpacing = 0.68f;
}
