using UnityEngine;
using TMPro;

public class MoneyUI : MonoBehaviour
{
    [Header("Money Text")]
    public TMP_Text moneyText;

    void Update()
    {
        if (DataManager.instance == null || DataManager.instance.gameData == null) return;
        
        // long 최대값 초과 방지
        if (DataManager.instance.gameData.money > long.MaxValue - 1)
            DataManager.instance.gameData.money = long.MaxValue - 1;
        
        moneyText.text = $"{DataManager.instance.gameData.money:N0}$";
    }
}