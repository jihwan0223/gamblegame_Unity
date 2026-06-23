using UnityEngine;
using TMPro;

public class MoneyUI : MonoBehaviour
{
    [Header("Money Text")]
    public TMP_Text moneyText;

    void Update()
    {
        if (DataManager.instance == null || DataManager.instance.gameData == null) return;
        moneyText.text = $"{FormatMoney(DataManager.instance.gameData.money)}$";
    }

    private string FormatMoney(long amount)
    {
        if (amount >= 1_000_000_000_000_000) return $"{amount / 1_000_000_000_000_000.0:F2}Qa";
        if (amount >= 1_000_000_000_000)     return $"{amount / 1_000_000_000_000.0:F2}T";
        if (amount >= 1_000_000_000)         return $"{amount / 1_000_000_000.0:F2}B";
        if (amount >= 1_000_000)             return $"{amount / 1_000_000.0:F2}M";
        if (amount >= 1_000)                 return $"{amount / 1_000.0:F2}K";
        return $"{amount:N0}";
    }
}