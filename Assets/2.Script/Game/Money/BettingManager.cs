using UnityEngine;
using TMPro;

public class BettingManager : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI bettingMessageText;
    public TextMeshProUGUI currentBettingText;
    public TMP_InputField inputField;

    [Header("Manager")]
    public GameManager gameManager;

    private long currentBet = 0;
    private bool isBetDone = false;

    // 베팅 버튼
    public void OnClickBet()
    {
        if(isBetDone) return;

        if(string.IsNullOrEmpty(inputField.text)) return;

        if (!long.TryParse(inputField.text, out long bet))
        {
            if(LanguageToggle.Instance._isKorean) bettingMessageText.text = "베팅 금액 입력";
            else bettingMessageText.text = "Enter betting amount";
            return;
        }

        if (bet <= 0 || bet > DataManager.instance.gameData.money)
        {
            if(LanguageToggle.Instance._isKorean) bettingMessageText.text = "올바른 베팅 금액을 입력하세요!";
            else bettingMessageText.text = "Please enter the correct betting amount..!";
            return;

        }

        currentBet = bet;
        DataManager.instance.gameData.money -= currentBet;
        DataManager.instance.SaveGameData();

        Debug.Log("베팅 완료 : " + currentBet);
        if(LanguageToggle.Instance._isKorean) currentBettingText.text = $"베팅 : {currentBet}";
        else currentBettingText.text = $"betting : {currentBet}";

        isBetDone = true;

        gameManager.StartGame();
    }

    // 승리
    public void OnGameWin(float percent)
    {
        if (currentBet <= 0) return;
    
        long profit = (long)(currentBet * (percent / 100f));
        long reward = currentBet + profit;
    
        // 업그레이드 적용
        reward = UpgradeManager.instance.CalcWinReward(reward);
    
        DataManager.instance.gameData.money += reward;
        DataManager.instance.SaveGameData();
    
        bettingMessageText.text = $"+{reward}$";
    
        SoundManager.instance.PlayerWin();
        ResetBet();
    }
    
    // 패배
    public void OnGameLose()
    {
        if (currentBet <= 0) return;

        // 베팅 시 이미 차감됐으므로 손실 감소분만 돌려줌
        long actualLoss = UpgradeManager.instance.CalcLoss(currentBet);
        long savedAmount = currentBet - actualLoss; // 감소로 아낀 금액

        // 손실 감소 적용 (아낀 금액 돌려주기)
        DataManager.instance.gameData.money += savedAmount;

        // 환급 계산
        long refund = UpgradeManager.instance.CalcLossRefund(actualLoss);
        DataManager.instance.gameData.money += refund;

        DataManager.instance.SaveGameData();

        bettingMessageText.text = refund > 0
            ? $"-{actualLoss}$ (+{refund}$ refund)"
            : $"-{actualLoss}$";

        SoundManager.instance.PlayerLose();
        ResetBet();
    }
    
    // 무승부
    public void OnGameDraw()
    {
        if (currentBet <= 0) return;
    
        DataManager.instance.gameData.money += currentBet;
        DataManager.instance.SaveGameData();
    
        bettingMessageText.text = $"+{0}$";
        ResetBet();
    }

    public void ResetBet()
    {
        currentBet = 0;
        isBetDone = false;
        currentBettingText.text = "";
    }
}