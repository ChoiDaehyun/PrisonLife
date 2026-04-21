using UnityEngine;
using UnityEngine.UI;

public class PlayerMoneyTextUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerMoneyInventory playerMoneyInventory;
    [SerializeField] private Text moneyText;

    private void OnEnable()
    {
        if (playerMoneyInventory != null)
        {
            playerMoneyInventory.OnMoneyChanged += HandleMoneyChanged;
        }
    }

    private void OnDisable()
    {
        if (playerMoneyInventory != null)
        {
            playerMoneyInventory.OnMoneyChanged -= HandleMoneyChanged;
        }
    }

    private void Start()
    {
        RefreshText();
    }

    private void HandleMoneyChanged(int currentMoney)
    {
        UpdateText(currentMoney);
    }

    private void RefreshText()
    {
        if (playerMoneyInventory == null)
        {
            return;
        }

        UpdateText(playerMoneyInventory.CurrentMoney);
    }

    private void UpdateText(int currentMoney)
    {
        if (moneyText == null)
        {
            return;
        }

        moneyText.text = $"{currentMoney}";
    }

    private void OnValidate()
    {
        if (moneyText == null)
        {
            moneyText = GetComponent<Text>();
        }
    }
}