using UnityEngine;

public class WorkerUnit : MonoBehaviour
{
    [Header("Runtime")]
    [SerializeField] private int assignedRow;
    [SerializeField] private int currentColumn;
    [SerializeField] private float heightOffset = 0.5f;
    [SerializeField] private float backOffset = 1f;
    [SerializeField] private WorkerPickaxeSwingAnimator pickaxeSwingAnimator;

    private WorkerSystemData workerSystemData;
    private OreGridCoordinateProvider gridProvider;
    private OreSpawnController oreSpawnController;
    private WarehouseInventory warehouseInventory;

    private IWorkerState currentState;
    public WorkerMoveState MoveState { get; private set; }
    public WorkerWaitState WaitState { get; private set; }

    public int AssignedRow => assignedRow;
    public int CurrentColumn => currentColumn;
    public float WorkDelay => workerSystemData != null ? workerSystemData.workDelay : 1f;

    public void Initialize(
        int row,
        WorkerSystemData data,
        OreGridCoordinateProvider grid,
        OreSpawnController oreSpawner,
        WarehouseInventory warehouse)
    {
        assignedRow = row;
        currentColumn = 0;
        workerSystemData = data;
        gridProvider = grid;
        oreSpawnController = oreSpawner;
        warehouseInventory = warehouse;

        MoveState = new WorkerMoveState(this);
        WaitState = new WorkerWaitState(this);

        SnapToCurrentCell();
        ChangeState(MoveState);
    }

    private void Awake()
    {
        Rigidbody rb = GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.useGravity = false;
            rb.isKinematic = true;
        }
    }

    private void Update()
    {
        currentState?.Update();
    }

    public void ChangeState(IWorkerState newState)
    {
        if (newState == null)
        {
            return;
        }

        if (currentState == newState)
        {
            return;
        }

        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }

    private Vector3 GetCellPosition()
    {
        Vector3 position = gridProvider.GetWorldPosition(assignedRow, currentColumn);

        position.y += heightOffset;

        Vector3 backDirection = -gridProvider.GridData.columnStep.normalized;
        position += backDirection * backOffset;

        return position;
    }

    public void MoveToCurrentCell()
    {
        if (gridProvider == null || workerSystemData == null)
        {
            return;
        }

        Vector3 targetPosition = GetCellPosition();

        Vector3 currentPosition = transform.position;
        Vector3 currentXZ = new Vector3(currentPosition.x, 0f, currentPosition.z);
        Vector3 targetXZ = new Vector3(targetPosition.x, 0f, targetPosition.z);

        Vector3 movedXZ = Vector3.MoveTowards(
            currentXZ,
            targetXZ,
            workerSystemData.moveSpeed * Time.deltaTime);

        transform.position = new Vector3(movedXZ.x, targetPosition.y, movedXZ.z);

        Vector3 lookDirection = targetPosition - transform.position;
        lookDirection.y = 0f;

        if (lookDirection.sqrMagnitude > 0.0001f)
        {
            transform.rotation = Quaternion.LookRotation(lookDirection);
        }

        float distanceSqr = (transform.position - targetPosition).sqrMagnitude;

        if (distanceSqr > 0.0001f)
        {
            return;
        }

        HandleArrivedAtCell();
    }

    public void TryMineCurrentCell()
    {
        if (oreSpawnController == null || warehouseInventory == null)
        {
            return;
        }

        OreNode oreNode = oreSpawnController.GetOreAt(assignedRow, currentColumn);

        if (oreNode == null || !oreNode.IsSpawned)
        {
            return;
        }

        oreSpawnController.MineOre(oreNode, 4f);
        warehouseInventory.AddOre(1);
    }

    public void AdvanceColumn()
    {
        if (gridProvider == null || gridProvider.GridData == null)
        {
            return;
        }

        currentColumn++;

        if (currentColumn >= gridProvider.GridData.columnCount)
        {
            currentColumn = 0;
        }
    }

    private void HandleArrivedAtCell()
    {
        if (oreSpawnController == null)
        {
            return;
        }

        OreNode oreNode = oreSpawnController.GetOreAt(assignedRow, currentColumn);

        if (oreNode != null && oreNode.IsSpawned)
        {
            ChangeState(WaitState);
            return;
        }

        AdvanceColumn();
        ChangeState(MoveState);
    }

    private void SnapToCurrentCell()
    {
        if (gridProvider == null)
        {
            return;
        }

        Vector3 position = GetCellPosition();
        transform.position = position;
    }

    public void StartMiningAnimation()
    {
        if (pickaxeSwingAnimator != null)
        {
            pickaxeSwingAnimator.SetSwinging(true);
        }
    }

    public void StopMiningAnimation()
    {
        if (pickaxeSwingAnimator != null)
        {
            pickaxeSwingAnimator.SetSwinging(false);
        }
    }
}