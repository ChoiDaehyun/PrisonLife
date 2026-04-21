using UnityEngine;

[RequireComponent(typeof(Collider))]
public abstract class InstallmentPurchaseZoneBase : MonoBehaviour
{
    [Header("References")]
    [SerializeField] protected DepositPadGaugeView gaugeView;

    [Header("Deposit Settings")]
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private int depositUnit = 10;
    [SerializeField] private float depositInterval = 0.15f;

    [Header("Runtime")]
    [SerializeField] protected int submittedAmount;

    private float depositTimer;

    protected abstract int GetRequiredCost();
    protected abstract bool TryCompletePurchase(Collider other);

    protected virtual void Awake()
    {
        RefreshUI();
    }

    protected virtual void Reset()
    {
        Collider zoneCollider = GetComponent<Collider>();
        zoneCollider.isTrigger = true;
    }

    protected virtual void OnEnable()
    {
        depositTimer = 0f;
        RefreshUI();
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag(playerTag))
        {
            return;
        }

        int requiredCost = GetRequiredCost();

        if (requiredCost <= 0)
        {
            return;
        }

        if (submittedAmount >= requiredCost)
        {
            return;
        }

        PlayerMoneyInventory moneyInventory =
            other.GetComponent<PlayerMoneyInventory>() ??
            other.GetComponentInParent<PlayerMoneyInventory>();

        if (moneyInventory == null)
        {
            return;
        }

        depositTimer += Time.deltaTime;

        if (depositTimer < depositInterval)
        {
            return;
        }

        depositTimer = 0f;

        int remainingCost = requiredCost - submittedAmount;
        int currentDepositAmount = Mathf.Min(depositUnit, remainingCost);

        bool spent = moneyInventory.TrySpendMoney(currentDepositAmount);

        if (!spent)
        {
            return;
        }

        submittedAmount += currentDepositAmount;
        submittedAmount = Mathf.Clamp(submittedAmount, 0, requiredCost);

        RefreshUI();

        if (submittedAmount < requiredCost)
        {
            return;
        }

        bool completed = TryCompletePurchase(other);

        if (!completed)
        {
            return;
        }

        gameObject.SetActive(false);
    }

    protected void RefreshUI()
    {
        if (gaugeView == null)
        {
            return;
        }

        gaugeView.UpdateView(submittedAmount, GetRequiredCost());
    }

    protected virtual void OnValidate()
    {
        Collider zoneCollider = GetComponent<Collider>();

        if (depositUnit <= 0)
        {
            depositUnit = 10;
        }

        if (depositInterval <= 0f)
        {
            depositInterval = 0.15f;
        }
    }
}
