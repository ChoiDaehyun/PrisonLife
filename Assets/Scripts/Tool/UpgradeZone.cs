using UnityEngine;

[RequireComponent(typeof(Collider))]
public class UpgradeZone : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerMoneyInventory playerMoneyInventory;
    [SerializeField] private DepositPadGaugeView depositPadGaugeView;
    [SerializeField] private DelayedCanvasShow delayedCanvasShow;

    [Header("Upgrade Targets")]
    [SerializeField] private GameObject objectToActivate;
    [SerializeField] private GameObject objectToDeactivate;

    [Header("Settings")]
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private int purchaseCost = 50;
    [SerializeField] private int submitUnit = 10;
    [SerializeField] private float submitInterval = 0.2f;

    [Header("Runtime")]
    [SerializeField] private int currentSubmittedMoney;
    [SerializeField] private bool isPurchased;

    private float submitTimer;

    private void Start()
    {
        RefreshGauge();
    }

    private void Reset()
    {
        Collider zoneCollider = GetComponent<Collider>();
        zoneCollider.isTrigger = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (isPurchased)
        {
            return;
        }

        if (!other.CompareTag(playerTag))
        {
            return;
        }

        PlayerMoneyInventory moneyInventory = playerMoneyInventory;

        if (moneyInventory == null)
        {
            moneyInventory =
                other.GetComponent<PlayerMoneyInventory>() ??
                other.GetComponentInParent<PlayerMoneyInventory>();
        }

        if (moneyInventory == null)
        {
            return;
        }

        submitTimer += Time.deltaTime;

        if (submitTimer < submitInterval)
        {
            return;
        }

        submitTimer = 0f;

        TrySubmitMoney(moneyInventory);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(playerTag))
        {
            return;
        }

        submitTimer = 0f;
    }

    private void TrySubmitMoney(PlayerMoneyInventory moneyInventory)
    {
        if (moneyInventory == null)
        {
            return;
        }

        if (currentSubmittedMoney >= purchaseCost)
        {
            CompletePurchase();
            return;
        }

        int remainingCost = purchaseCost - currentSubmittedMoney;
        int submitAmount = Mathf.Min(submitUnit, remainingCost);

        bool spent = moneyInventory.TrySpendMoney(submitAmount);

        if (!spent)
        {
            return;
        }

        currentSubmittedMoney += submitAmount;

        RefreshGauge();

        if (currentSubmittedMoney >= purchaseCost)
        {
            CompletePurchase();
        }
    }

    private void CompletePurchase()
    {
        if (isPurchased)
        {
            return;
        }

        isPurchased = true;

        if (objectToActivate != null)
        {
            objectToActivate.SetActive(true);
        }

        if (objectToDeactivate != null)
        {
            objectToDeactivate.SetActive(false);
        }

        if (delayedCanvasShow != null)
        {
            delayedCanvasShow.ShowCanvasAfterDelay();
        }

        RefreshGauge();

        gameObject.SetActive(false);
    }

    private void RefreshGauge()
    {
        if (depositPadGaugeView != null)
        {
            depositPadGaugeView.UpdateView(currentSubmittedMoney, purchaseCost);
        }
    }

    private void OnValidate()
    {
        Collider zoneCollider = GetComponent<Collider>();

        if (purchaseCost <= 0)
        {
            purchaseCost = 50;
        }

        if (submitUnit <= 0)
        {
            submitUnit = 10;
        }

        if (submitInterval <= 0f)
        {
            submitInterval = 0.2f;
        }
    }
}
