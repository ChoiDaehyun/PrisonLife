using UnityEngine;

public class OreGridCoordinateProvider : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private OreGridData gridData;

    public OreGridData GridData => gridData; //public getter(읽기 전용)

    [Header("Debug")]
    [SerializeField] private bool drawGizmos = true; //셀 위치 디버깅 용
    [SerializeField] private float gizmoSphereRadius = 0.08f; //셀 구 크기

    private Vector3[,] cachedPositions; //각 셀 월드 좌표 저장(2차원 배열)

    private void Awake()
    {
        BuildGridPositions();
    }
    //그리드 전체 좌표 계산 후 캐시에 저장
    public void BuildGridPositions()
    {
        if (gridData == null)
        {
            return;
        }

        cachedPositions = new Vector3[gridData.rowCount, gridData.columnCount];

        for (int row = 0; row < gridData.rowCount; row++)
        {
            for (int col = 0; col < gridData.columnCount; col++)
            {
                cachedPositions[row, col] = gridData.origin
                                            + gridData.rowStep * row
                                            + gridData.columnStep * col;
            }
        }
    }
    //좌표 get
    public Vector3 GetWorldPosition(int row, int col)
    {
        if (cachedPositions == null)
        {
            BuildGridPositions();
        }
        if (!IsValidCell(row, col))
        {
            return Vector3.zero;
        }

        return cachedPositions[row, col];
    }
    //그리드 범위 검사
    public bool IsValidCell(int row, int col)
    {
        if (gridData == null)
        {
            return false;
        }

        return row >= 0 && row < gridData.rowCount &&
                col >= 0 && col < gridData.columnCount;
    }
    //셀 위치 확인 디버깅용
    private void OnDrawGizmos()
    {
        if (!drawGizmos || gridData == null)
        {
            return;
        }

        Gizmos.color = Color.yellow;

        for (int row = 0; row < gridData.rowCount; row++)
        {
            for (int col = 0; col < gridData.columnCount; col++)
            {
                Vector3 position = gridData.origin
                                + gridData.rowStep * row
                                + gridData.columnStep * col;

                Gizmos.DrawSphere(position, gizmoSphereRadius);
            }
        }
    }
    //검증
    private void OnValidate()
    {
        if (gridData == null)
        {

        }
    }
}
