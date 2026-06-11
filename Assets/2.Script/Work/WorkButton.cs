using UnityEngine;
using TMPro;
using System.Collections;

public class WorkButton : MonoBehaviour
{
    [Header("Refs")]
    public PlayerHand playerHand;
    public DeckManager workDeckManager;
    public WorkCardCleaner cleaner;

    [Header("Money UI")]
    public TMP_Text moneyText;
    public TMP_Text currentMoneyText;
    private Coroutine _textCoroutine;

    public long rewardPerCard = 1;

    // 버튼 클릭
    public void OnClickWork()
    {
        if (DataManager.instance == null) return;

        Card newCard = workDeckManager.DrawCard();

        if (newCard != null)
        {
            playerHand.AddCard(newCard);

            long cardValue = newCard.rank >= 11 ? 10 : newCard.rank;
            long income    = UpgradeManager.instance.CalcWorkIncome(cardValue);
            DataManager.instance.gameData.money += income;
            DataManager.instance.SaveGameData();

            long bonus = income - cardValue;

            // 업그레이드 여부 상관없이 항상 표시
            if (bonus > 0)
                currentMoneyText.SetText(LanguageToggle.Instance._isKorean
                    ? $"+{income}$ (수입 증가 +{bonus}$)"
                    : $"+{income}$ (Work Boost +{bonus}$)");
            else
                currentMoneyText.SetText($"+{income}$");

            UpdateMoneyUI();
        }

        if (cleaner != null) cleaner.CheckAndClear();
    }


    private void ShowIncomeText(string msg)
    {
        if (_textCoroutine != null) StopCoroutine(_textCoroutine);
        _textCoroutine = StartCoroutine(ShowTextRoutine(msg));
    }

    private IEnumerator ShowTextRoutine(string msg)
    {
        currentMoneyText.SetText(msg);
        yield return new WaitForSeconds(1f);
        currentMoneyText.SetText("Work!");
    }

    
    // 돈 UI 갱신
    private void UpdateMoneyUI()
    {
        if (moneyText == null) return;

        moneyText.text = $"Money : {DataManager.instance.gameData.money}$";
    }
}