using UnityEngine;

public class OreNode : MonoBehaviour
{
    public int Row { get; private set; } //행 idx
    public int Column { get; private set; } //열 idx
    public bool IsSpawned { get; private set; }

    public void Spawn(int row, int col, Vector3 worldPosition)
    {
        Row = row;
        Column = col;
        IsSpawned = true;

        transform.position = worldPosition;
        gameObject.SetActive(true); //활성화
    }

    public void Despawn()
    {
        IsSpawned = false;
        gameObject.SetActive(false); //비활성화
    }
}
