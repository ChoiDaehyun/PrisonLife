using UnityEngine;

public class WorkerWaitState : IWorkerState
{
    private readonly WorkerUnit workerUnit;
    private float waitTimer;

    public WorkerWaitState(WorkerUnit workerUnit)
    {
        this.workerUnit = workerUnit;
    }

    public void Enter()
    {
        waitTimer = workerUnit.WorkDelay;
        workerUnit.TryMineCurrentCell();
    }

    public void Update()
    {
        waitTimer -= Time.deltaTime;

        if (waitTimer > 0f)
        {
            return;
        }

        workerUnit.StopMiningAnimation();
        workerUnit.TryMineCurrentCell();
        workerUnit.AdvanceColumn();
        workerUnit.ChangeState(workerUnit.MoveState);
    }

    public void Exit()
    {
        workerUnit.StopMiningAnimation();
    }
}