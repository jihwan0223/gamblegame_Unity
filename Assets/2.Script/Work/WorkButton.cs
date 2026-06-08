using UnityEngine;
using TMPro;

public class WorkButton : MonoBehaviour
{
    [Header("Refs")]
    public PlayerHand playerHand;
    public DeckManager workDeckManager;
    public WorkCardCleaner cleaner;

    [Header("Money UI")]
    public TMP_Text moneyText;

    public long rewardPerCard = 1;

    // 버튼 클릭
    public void OnClickWork()
    {
        if (DataManager.instance == null) return;
    
        Card newCard = workDeckManager.DrawCard();
    
        if (newCard != null)
        {
            playerHand.AddCard(newCard);
    
            // 업그레이드 적용 (알바 수입 증가)
            long income = UpgradeManager.instance.CalcWorkIncome(rewardPerCard);
            DataManager.instance.gameData.money += income;
            DataManager.instance.SaveGameData();
    
            UpdateMoneyUI();
        }

        // 카드 너무 많으면 정리
        if (cleaner != null)
        {
            cleaner.CheckAndClear();
        }
    }

    // 돈 UI 갱신
    private void UpdateMoneyUI()
    {
        if (moneyText == null) return;

        moneyText.text = $"Money : {DataManager.instance.gameData.money}$";
    }
}