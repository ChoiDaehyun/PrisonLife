using UnityEngine;
using UnityEngine.UI;

public class PrisonerJailCountTextUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PrisonerJailInventory jailInventory;
    [SerializeField] private TextMesh jailCountText;

    private void OnEnable()
    {
        if (jailInventory != null)
        {
            jailInventory.OnJailedCountChanged += HandleJailedCountChanged;
        }
    }

    private void OnDisable()
    {
        if (jailInventory != null)
        {
            jailInventory.OnJailedCountChanged -= HandleJailedCountChanged;
        }
    }

    private void Start()
    {
        Refresh();
    }

    private void HandleJailedCountChanged(int currentCount, int maxCount)
    {
        UpdateText(currentCount, maxCount);
    }

    private void Refresh()
    {
        if (jailInventory == null || jailCountText == null)
        {
            return;
        }

        UpdateText(jailInventory.CurrentJailedCount, jailInventory.MaxJailedCount);
    }

    private void UpdateText(int currentCount, int maxCount)
    {
        if (jailCountText == null)
        {
            return;
        }

        jailCountText.text = $"{currentCount} / {maxCount}";
    }
}