using UnityEngine;
using System;

public class PrisonerQueueManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PrisonerQueueData queueData;
    [SerializeField] private PrisonerUnit prisonerPrefab;
    [SerializeField] private HandcuffStorage submittedHandcuffStorage;
    [SerializeField] private MoneyStorage moneyStorage;
    [SerializeField] private PrisonerJailInventory jailInventory;

    [Header("Runtime")]
    [SerializeField] private PrisonerUnit outsideWaitPrisoner; // 1
    [SerializeField] private PrisonerUnit insideWaitPrisoner;  // 2
    [SerializeField] private PrisonerUnit servicePrisoner;     // 3

    private float handcuffGiveTimer;

    public event Action<PrisonerUnit> OnServicePrisonerChanged;

    public PrisonerUnit CurrentServicePrisoner => servicePrisoner;
    private void Awake()
    {
        if (jailInventory != null && queueData != null)
        {
            jailInventory.InitializeMaxCount(queueData.maxJailedCount);
        }
    }

    private void Start()
    {
        SpawnInitialQueue();
    }

    private void Update()
    {
        TryAutoGiveHandcuffs();
    }

    private void SpawnInitialQueue()
    {
        outsideWaitPrisoner = CreatePrisonerAt(queueData.outsideWaitPosition, false);
        insideWaitPrisoner = CreatePrisonerAt(queueData.insideWaitPosition, false);
        servicePrisoner = CreatePrisonerAt(queueData.serviceWaitPosition, true);

        NotifyServicePrisonerChanged();
    }

    private PrisonerUnit CreatePrisonerAt(Vector3 position, bool isServicePoint)
    {
        PrisonerUnit prisoner = Instantiate(prisonerPrefab, transform);
        prisoner.Initialize(queueData, GetRandomRequiredHandcuffCount());
        prisoner.SnapToPosition(position, isServicePoint);
        prisoner.OnFinishedPath += HandlePrisonerFinishedPath;
        prisoner.OnReachedServicePoint += HandlePrisonerReachedServicePoint;

        PrisonerNeedHandcuffTextUI needTextUI = prisoner.GetComponentInChildren<PrisonerNeedHandcuffTextUI>();

        if (needTextUI != null)
        {
            needTextUI.Initialize(prisoner, this);
        }

        return prisoner;
    }

    private int GetRandomRequiredHandcuffCount()
    {
        return UnityEngine.Random.Range(queueData.minRequiredHandcuffs, queueData.maxRequiredHandcuffs + 1);
    }

    private void TryAutoGiveHandcuffs()
    {
        if (servicePrisoner == null)
        {
            return;
        }

        if (!servicePrisoner.IsAtServicePoint)
        {
            return;
        }

        if (servicePrisoner.IsMovingToJail)
        {
            return;
        }

        if (servicePrisoner.HasReceivedAllHandcuffs())
        {
            TryReleaseServicePrisoner();
            return;
        }

        handcuffGiveTimer += Time.deltaTime;

        if (handcuffGiveTimer < queueData.handcuffGiveInterval)
        {
            return;
        }

        handcuffGiveTimer = 0f;

        if (submittedHandcuffStorage == null || moneyStorage == null)
        {
            return;
        }

        bool consumed = submittedHandcuffStorage.TryConsumeSubmittedHandcuffs(1);

        if (!consumed)
        {
            return;
        }

        bool received = servicePrisoner.ReceiveOneHandcuff();

        if (!received)
        {
            return;
        }

        moneyStorage.AddMoney(queueData.moneyPerHandcuff);

        if (servicePrisoner.HasReceivedAllHandcuffs())
        {
            TryReleaseServicePrisoner();
        }
    }

    private void TryReleaseServicePrisoner()
    {
        if (servicePrisoner == null)
        {
            return;
        }

        if (jailInventory != null && jailInventory.IsFull)
        {
            return;
        }

        Vector3 jailPosition = queueData.jailBasePosition;

        servicePrisoner.MoveToJailPath(queueData.turnPointPosition, jailPosition);

        ShiftQueueForward();
    }

    private void ShiftQueueForward()
    {
        PrisonerUnit releasedPrisoner = servicePrisoner;

        servicePrisoner = insideWaitPrisoner;
        insideWaitPrisoner = outsideWaitPrisoner;
        outsideWaitPrisoner = CreatePrisonerAt(queueData.outsideWaitPosition, false);

        if (servicePrisoner != null)
        {
            servicePrisoner.MoveToSinglePoint(queueData.serviceWaitPosition, true);
        }

        if (insideWaitPrisoner != null)
        {
            insideWaitPrisoner.MoveToSinglePoint(queueData.insideWaitPosition, false);
        }

        if (outsideWaitPrisoner != null)
        {
            outsideWaitPrisoner.SnapToPosition(queueData.outsideWaitPosition, false);
        }

        handcuffGiveTimer = 0f;
        NotifyServicePrisonerChanged();
    }

    private Vector3 GetJailSlotPosition(int jailIndex)
    {
        int row = jailIndex / queueData.jailRowSize;
        int col = jailIndex % queueData.jailRowSize;

        Vector3 offset = new Vector3(
            col * queueData.jailSpacingX,
            0f,
            -row * queueData.jailSpacingZ);

        return queueData.jailBasePosition + offset;
    }

    private void HandlePrisonerReachedServicePoint(PrisonerUnit prisoner)
    {
        if (prisoner == servicePrisoner)
        {
            handcuffGiveTimer = 0f;
            NotifyServicePrisonerChanged();
        }
    }

    private void HandlePrisonerFinishedPath(PrisonerUnit prisoner)
    {
        if (prisoner == null)
        {
            return;
        }
    }

    private void NotifyServicePrisonerChanged()
    {
        OnServicePrisonerChanged?.Invoke(servicePrisoner);
    }
}