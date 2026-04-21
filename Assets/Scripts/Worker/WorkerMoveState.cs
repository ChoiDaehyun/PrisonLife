using UnityEngine;

public class WorkerMoveState : IWorkerState
{
    private readonly WorkerUnit workerUnit;

    public WorkerMoveState(WorkerUnit workerUnit)
    {
        this.workerUnit = workerUnit;
    }

    public void Enter()
    {

    }

    public void Update()
    {
        workerUnit.MoveToCurrentCell();
    }

    public void Exit()
    {

    }
}