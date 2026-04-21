using UnityEngine;

[CreateAssetMenu(
    fileName = "PlayerInventoryData",
    menuName = "Game/Player Inventory Data"
)]
public class PlayerInventoryData : ScriptableObject
{
    [Header("Ore Capacity")]
    public int maxOreCapacity = 10;
}
