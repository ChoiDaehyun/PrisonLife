using UnityEngine;

public class DepositPadGaugeView : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform gaugeFillPlane;
    [SerializeField] private TextMesh remainingMoneyText;

    [Header("Settings")]
    [SerializeField] private bool hideTextWhenCompleted = true;

    private Vector3 fullGaugeScale;
    private Vector3 fullGaugeLocalPosition;
    private bool isInitialized;

    private void Awake()
    {
        CacheFullGaugeState();
    }

    private void OnEnable()
    {
        CacheFullGaugeState();
    }

    private void CacheFullGaugeState()
    {
        if (gaugeFillPlane == null)
        {
            Debug.LogWarning("[DepositPadGaugeView] Gauge Fill Plane reference is missing.");
            isInitialized = false;
            return;
        }

        fullGaugeScale = gaugeFillPlane.localScale;
        fullGaugeLocalPosition = gaugeFillPlane.localPosition;
        isInitialized = true;

        Debug.Log($"[DepositPadGaugeView] Cached full state. Scale: {fullGaugeScale}, Pos: {fullGaugeLocalPosition}");
    }

    public void UpdateView(int submittedAmount, int requiredAmount)
    {
        if (!isInitialized)
        {
            CacheFullGaugeState();
        }

        if (requiredAmount <= 0)
        {
            requiredAmount = 1;
        }

        submittedAmount = Mathf.Clamp(submittedAmount, 0, requiredAmount);

        float ratio = (float)submittedAmount / requiredAmount;
        int remainingAmount = Mathf.Max(0, requiredAmount - submittedAmount);

        UpdateGauge(ratio);
        UpdateText(remainingAmount);
    }

    private void UpdateGauge(float ratio)
    {
        if (gaugeFillPlane == null)
        {
            return;
        }

        Vector3 newScale = fullGaugeScale;
        newScale.x = fullGaugeScale.z * ratio;
        gaugeFillPlane.localScale = newScale;

        float lostSize = fullGaugeScale.x - newScale.x;
        Vector3 newPosition = fullGaugeLocalPosition;
        newPosition.z = fullGaugeLocalPosition.z - lostSize * 0.5f;
        gaugeFillPlane.localPosition = newPosition;
    }

    private void UpdateText(int remainingAmount)
    {
        if (remainingMoneyText == null)
        {
            return;
        }

        bool shouldShow = remainingAmount > 0 || !hideTextWhenCompleted;
        remainingMoneyText.gameObject.SetActive(shouldShow);

        if (!shouldShow)
        {
            return;
        }

        remainingMoneyText.text = remainingAmount.ToString();
    }
}