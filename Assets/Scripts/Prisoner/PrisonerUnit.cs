using System;
using System.Collections.Generic;
using UnityEngine;

public class PrisonerUnit : MonoBehaviour
{
    [Header("Runtime")]
    [SerializeField] private int requiredHandcuffCount;
    [SerializeField] private int receivedHandcuffCount;
    [SerializeField] private bool isAtServicePoint;
    [SerializeField] private bool isMovingToJail;
    [SerializeField] private bool isJailRegistered;

    private PrisonerQueueData queueData;
    private Queue<Vector3> movementQueue = new Queue<Vector3>();
    private Vector3 currentTarget;
    private bool hasTarget;

    private Rigidbody prisonerRigidbody;

    public event Action<PrisonerUnit> OnReachedServicePoint;
    public event Action<PrisonerUnit> OnFinishedPath;
    public event Action<int, int> OnHandcuffProgressChanged;
    public event Action<PrisonerUnit> OnPrisonerStateChanged;

    public int RequiredHandcuffCount => requiredHandcuffCount;
    public int ReceivedHandcuffCount => receivedHandcuffCount;
    public int RemainingHandcuffCount => Mathf.Max(0, requiredHandcuffCount - receivedHandcuffCount);
    public bool IsAtServicePoint => isAtServicePoint;
    public bool IsMovingToJail => isMovingToJail;
    public bool IsJailRegistered => isJailRegistered;

    public void Initialize(PrisonerQueueData data, int requiredCount)
    {
        queueData = data;
        requiredHandcuffCount = Mathf.Max(1, requiredCount);
        receivedHandcuffCount = 0;
        isAtServicePoint = false;
        isMovingToJail = false;
        isJailRegistered = false;
        movementQueue.Clear();
        hasTarget = false;

        NotifyHandcuffProgressChanged();
        NotifyPrisonerStateChanged();

        SetPhysicsActive(false);
    }

    private void Awake()
    {
        prisonerRigidbody = GetComponent<Rigidbody>();

        SetPhysicsActive(false);
    }

    private void Update()
    {
        if (!hasTarget || queueData == null)
        {
            return;
        }

        transform.position = Vector3.MoveTowards(
            transform.position,
            currentTarget,
            queueData.moveSpeed * Time.deltaTime);

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

        HandleReachedTarget();
    }

    public void SnapToPosition(Vector3 position, bool markServicePoint = false)
    {
        transform.position = position;
        isAtServicePoint = markServicePoint;
        hasTarget = false;
        movementQueue.Clear();

        NotifyPrisonerStateChanged();
    }

    public void MoveToSinglePoint(Vector3 target, bool markServicePointOnArrival = false)
    {
        movementQueue.Clear();
        movementQueue.Enqueue(target);

        isAtServicePoint = false;
        hasTarget = false;

        if (markServicePointOnArrival)
        {
            movementQueue.Enqueue(new Vector3(float.NaN, 1f, 0f)); // marker
        }

        NotifyPrisonerStateChanged();
        MoveNext();
    }

    public void MoveToJailPath(Vector3 turnPoint, Vector3 jailPosition)
    {
        movementQueue.Clear();
        movementQueue.Enqueue(turnPoint);
        movementQueue.Enqueue(jailPosition);

        isAtServicePoint = false;
        isMovingToJail = true;
        hasTarget = false;

        NotifyPrisonerStateChanged();
        MoveNext();
    }

    public bool ReceiveOneHandcuff()
    {
        if (receivedHandcuffCount >= requiredHandcuffCount)
        {
            return false;
        }

        receivedHandcuffCount++;

        NotifyHandcuffProgressChanged();

        return true;
    }

    public bool HasReceivedAllHandcuffs()
    {
        return receivedHandcuffCount >= requiredHandcuffCount;
    }

    public void MarkJailRegistered()
    {
        isJailRegistered = true;
        NotifyPrisonerStateChanged();
    }

    private void MoveNext()
    {
        if (movementQueue.Count == 0)
        {
            hasTarget = false;
            OnFinishedPath?.Invoke(this);
            return;
        }

        Vector3 next = movementQueue.Dequeue();

        if (float.IsNaN(next.x))
        {
            isAtServicePoint = true;
            NotifyPrisonerStateChanged();
            OnReachedServicePoint?.Invoke(this);
            MoveNext();
            return;
        }

        currentTarget = next;
        hasTarget = true;
    }

    private void HandleReachedTarget()
    {
        hasTarget = false;

        if ((currentTarget - queueData.serviceWaitPosition).sqrMagnitude < 0.0001f)
        {
            isAtServicePoint = true;
            NotifyPrisonerStateChanged();
            OnReachedServicePoint?.Invoke(this);
        }

        MoveNext();
    }

    public void SetPhysicsActive(bool active)
    {
        if (prisonerRigidbody == null)
        {
            return;
        }

        prisonerRigidbody.isKinematic = !active;
        prisonerRigidbody.useGravity = active;
    }
    private void NotifyHandcuffProgressChanged()
    {
        OnHandcuffProgressChanged?.Invoke(receivedHandcuffCount, requiredHandcuffCount);
    }

    private void NotifyPrisonerStateChanged()
    {
        OnPrisonerStateChanged?.Invoke(this);
    }
}