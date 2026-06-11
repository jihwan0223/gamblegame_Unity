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
        long baseIncome = rewardPerCard;
        long income = UpgradeManager.instance.CalcWorkIncome(baseIncome);
        DataManager.instance.gameData.money += income;
        DataManager.instance.SaveGameData();

        // 증가분 표시
        long bonus = income - baseIncome;
        if (bonus > 0)
        {
            // 알바 수입 증가 적용 메시지 (텍스트 오브젝트 있으면 표시)
            Debug.Log(LanguageToggle.Instance._isKorean
                ? $"+{income}$ (수입 증가 +{bonus}$)"
                : $"+{income}$ (Work Boost +{bonus}$)");
        }

        UpdateMoneyUI();

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