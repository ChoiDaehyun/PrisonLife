using System;
using UnityEngine;

public class CarryMachineUnit : MonoBehaviour
{
    private enum CarryMachineState
    {
        MovingToClaimPoint,
        WaitingAtClaimPoint,
        MovingToSubmitPoint
    }

    [Header("Runtime")]
    [SerializeField] private CarryMachineState currentState;
    [SerializeField] private bool isCarrying;
    [SerializeField] private int carriedAmount;

    private CarryMachineData data;
    private HandcuffStorage claimHandcuffStorage;
    private HandcuffStorage submitHandcuffStorage;

    private Vector3 currentTarget;

    public event Action<int> OnCarriedAmountChanged;

    public int CarriedAmount => carriedAmount;

    public void Initialize(
        CarryMachineData carryMachineData,
        HandcuffStorage claimStorage,
        HandcuffStorage submitStorage)
    {
        data = carryMachineData;
        claimHandcuffStorage = claimStorage;
        submitHandcuffStorage = submitStorage;

        transform.position = data.spawnPosition;

        isCarrying = false;
        carriedAmount = 0;
        NotifyCarriedAmountChanged();

        ChangeState(CarryMachineState.MovingToClaimPoint);

    }

    private void Update()
    {
        if (data == null || claimHandcuffStorage == null || submitHandcuffStorage == null)
        {
            return;
        }

        switch (currentState)
        {
            case CarryMachineState.MovingToClaimPoint:
                MoveToTarget();
                break;

            case CarryMachineState.WaitingAtClaimPoint:
                TryPickupAtPoint1();
                break;

            case CarryMachineState.MovingToSubmitPoint:
                MoveToTarget();
                break;
        }
    }

    private void ChangeState(CarryMachineState newState)
    {
        currentState = newState;

        switch (newState)
        {
            case CarryMachineState.MovingToClaimPoint:
                currentTarget = data.point1;
                break;

            case CarryMachineState.WaitingAtClaimPoint:
                currentTarget = data.point1;
                break;

            case CarryMachineState.MovingToSubmitPoint:
                currentTarget = data.point2;
                break;
        }
    }

    private void MoveToTarget()
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            currentTarget,
            data.moveSpeed * Time.deltaTime);

        Vector3 lookDirection = currentTarget - transform.position;
        lookDirection.y = 0f;

        if (lookDirection.sqrMagnitude > 0.0001f)
        {
            transform.rotation = Quaternion.LookRotation(lookDirection);
        }

        if ((transform.position - currentTarget).sqrMagnitude > 0.0001f)
        {
            return;
        }

        HandleArrived();
    }

    private void HandleArrived()
    {
        if (currentState == CarryMachineState.MovingToClaimPoint)
        {
            TryPickupAtPoint1();
            return;
        }

        if (currentState == CarryMachineState.MovingToSubmitPoint)
        {
            SubmitAtPoint2();
        }
    }

    private void TryPickupAtPoint1()
    {
        if (isCarrying)
        {
            ChangeState(CarryMachineState.MovingToSubmitPoint);
            return;
        }

        if (claimHandcuffStorage.ClaimStoredHandcuffCount < data.carryAmount)
        {
            ChangeState(CarryMachineState.WaitingAtClaimPoint);
            return;
        }

        bool consumed = claimHandcuffStorage.TryConsumeClaimHandcuffs(data.carryAmount);

        if (!consumed)
        {
            ChangeState(CarryMachineState.WaitingAtClaimPoint);
            return;
        }

        carriedAmount = data.carryAmount;
        isCarrying = true;
        NotifyCarriedAmountChanged();

        ChangeState(CarryMachineState.MovingToSubmitPoint);
    }

    private void SubmitAtPoint2()
    {
        if (!isCarrying || carriedAmount <= 0)
        {
            ChangeState(CarryMachineState.MovingToClaimPoint);
            return;
        }

        submitHandcuffStorage.AddSubmittedHandcuffs(carriedAmount);

        carriedAmount = 0;
        isCarrying = false;
        NotifyCarriedAmountChanged();

        ChangeState(CarryMachineState.MovingToClaimPoint);
    }

    private void NotifyCarriedAmountChanged()
    {
        OnCarriedAmountChanged?.Invoke(carriedAmount);
    }

}