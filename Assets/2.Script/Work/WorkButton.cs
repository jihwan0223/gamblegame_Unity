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

        // 카드 1장 생성
        Card newCard = workDeckManager.DrawCard();

        if (newCard != null)
        {
            playerHand.AddCard(newCard);

            // 돈 +1
            DataManager.instance.gameData.money += rewardPerCard;
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