using System.Collections;
using UnityEngine;

public class OreSpawnController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private OreGridCoordinateProvider gridProvider;
    [SerializeField] private OrePool orePool; //Object Pooling

    [Header("Runtime Data")]
    [SerializeField] private OreNode[,] spawnedOres; //셀별로 어떤 광물이 올라가 있는지 저장

    public OreGridCoordinateProvider GridProvider => gridProvider;
    private void Awake()
    {
        if (gridProvider == null || orePool == null)
        {
            return;
        }

        gridProvider.BuildGridPositions(); //좌표 계산
        orePool.Initialize(); //일단 비활성

        BuildSpawnArray();
        SpawnAllOres();
    }

    private void BuildSpawnArray()
    {
        int rowCount = gridProvider.GridData.rowCount;
        int columnCount = gridProvider.GridData.columnCount;

        spawnedOres = new OreNode[rowCount, columnCount];
    }

    public void SpawnAllOres()
    {
        int rowCount = gridProvider.GridData.rowCount;
        int columnCount = gridProvider.GridData.columnCount;

        for (int row = 0; row < rowCount; row++)
        {
            for (int col = 0; col < columnCount; col++)
            {
                SpawnOreAt(row, col);
            }
        }
    }

    //개별 셀 스폰
    public void SpawnOreAt(int row, int col)
    {
        //유효 셀인지 확인
        if (!gridProvider.IsValidCell(row, col))
        {
            return;
        }
        //이미 광물이 있는지 확인
        if (spawnedOres[row, col] != null && spawnedOres[row, col].IsSpawned)
        {
            return;
        }

        Vector3 spawnPosition = gridProvider.GetWorldPosition(row, col);
        OreNode oreNode = orePool.Get(); //풀에서 가져옴

        oreNode.Spawn(row, col, spawnPosition); //활성화
        spawnedOres[row, col] = oreNode;
    }

    public void DespawnOreAt(int row, int col)
    {
        if (!gridProvider.IsValidCell(row, col))
        {
            return;
        }

        OreNode oreNode = spawnedOres[row, col];

        if (oreNode == null || !oreNode.IsSpawned)
        {
            return;
        }

        orePool.Return(oreNode); //풀로 반환
        spawnedOres[row, col] = null; //셀 비움
    }

    public void MineOre(OreNode oreNode, float respawnDelay)
    {
        if (oreNode == null)
        {
            return;
        }
        int row = oreNode.Row;
        int col = oreNode.Column;

        DespawnOreAt(row, col);
        StartCoroutine(RespawnOreRoutine(row, col, respawnDelay));
    }

    private IEnumerator RespawnOreRoutine(int row, int col, float delay)
    {

        yield return new WaitForSeconds(delay);

        SpawnOreAt(row, col);
    }

    //현재 광물 객체 반환
    public OreNode GetOreAt(int row, int col)
    {
        if (!gridProvider.IsValidCell(row, col))
        {
            return null;
        }

        return spawnedOres[row, col];
    }
}
