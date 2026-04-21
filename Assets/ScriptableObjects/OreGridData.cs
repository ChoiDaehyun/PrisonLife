using UnityEngine;

[CreateAssetMenu(
    fileName = "OreGridData",
    menuName = "Game/Ore Grid Data"
)]
public class OreGridData : ScriptableObject
{
    [Header("Grid Size")]
    public int rowCount = 8;
    public int columnCount = 17;

    [Header("Grid World Settings")]
    public Vector3 origin = new Vector3(-2.48000002f, 0f, 2.52999997f); //기준점
    public Vector3 rowStep = new Vector3(0.76f, 0f, -0.68f); //행 간격
    public Vector3 columnStep = new Vector3(0.68f, 0f, 0.76f); //열 간격

    public int TotalCellCount => rowCount * columnCount;
}