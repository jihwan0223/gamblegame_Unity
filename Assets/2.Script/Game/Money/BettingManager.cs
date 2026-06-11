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

    [Header("Next Card Button")]
    [SerializeField] private GameObject nextCardButton;

    private long currentBet = 0;
    private bool isBetDone = false;

    private bool _isAllIn = false;      // 올인


    private void Start()
    {
        inputField.onSubmit.AddListener((value) => OnClickBet());
    }
        
    // 베팅 버튼
    public void OnClickBet()
    {
        if (isBetDone) return;
        if (string.IsNullOrEmpty(inputField.text)) return;

        string cleanText = inputField.text.Replace(",", "").Trim();

        if (!long.TryParse(cleanText, out long bet))
        {
            bettingMessageText.text = LanguageToggle.Instance._isKorean
                ? "올바른 베팅 금액을 입력하세요!" : "Please enter the correct betting amount..!";
            return;
        }

        if (bet <= 0 || bet > DataManager.instance.gameData.money)
        {
            bettingMessageText.text = LanguageToggle.Instance._isKorean
                ? "올바른 베팅 금액을 입력하세요!" : "Please enter the correct betting amount..!";
            return;
        }

        // 올인 체크 (전재산 베팅)
        _isAllIn = bet == DataManager.instance.gameData.money;

        currentBet = bet;
        DataManager.instance.gameData.money -= currentBet;
        DataManager.instance.SaveGameData();

        if (LanguageToggle.Instance._isKorean)
            currentBettingText.text = _isAllIn ? $"올인!! : {currentBet:N0}" : $"베팅 : {currentBet:N0}";
        else
            currentBettingText.text = _isAllIn ? $"ALL IN!! : {currentBet:N0}" : $"Betting : {currentBet:N0}";

        isBetDone = true;
        gameManager.StartGame();

        // 6번 스킬 해금 시 다음 카드 버튼 활성화
        if (DataManager.instance.gameData.skillLevels[6] > 0)
            nextCardButton.SetActive(true);
    }

    // 승리
    public void OnGameWin(float percent)
    {
        if (currentBet <= 0) return;

        long profit = (long)(currentBet * (percent / 100f));
        long reward = currentBet + profit;
        long originalReward = reward;

        // 올인 보너스 적용
        if (_isAllIn)
            reward = UpgradeManager.instance.CalcAllInBonus(reward);

        reward = UpgradeManager.instance.CalcWinReward(reward);
        long bonus = reward - originalReward;

        DataManager.instance.gameData.money += reward;
        DataManager.instance.SaveGameData();

        string msg = $"+{reward}$";
        if (_isAllIn && bonus > 0)
            msg += LanguageToggle.Instance._isKorean
                ? $"\n(올인 보너스 +{bonus}$)"
                : $"\n(All-In Bonus +{bonus}$)";
        else if (bonus > 0)
            msg += LanguageToggle.Instance._isKorean
                ? $"\n(승리 보상 +{bonus}$)"
                : $"\n(Win Boost +{bonus}$)";

        bettingMessageText.text = msg;
        SoundManager.instance.PlayerWin();
        ResetBet();
    }

    // 패배
    public void OnGameLose()
    {
        if (currentBet <= 0) return;

        long actualLoss  = UpgradeManager.instance.CalcLoss(currentBet);
        long savedAmount = currentBet - actualLoss;
        DataManager.instance.gameData.money += savedAmount;
        DataManager.instance.SaveGameData();

        string msg;
        if (actualLoss == 0)
            msg = LanguageToggle.Instance._isKorean
                ? $"-0$\n(패배 환급 적용!)"
                : $"-0$\n(Loss Refund Applied!)";
        else if (savedAmount > 0)
            msg = LanguageToggle.Instance._isKorean
                ? $"-{actualLoss:N0}$\n(손실 감소 {savedAmount:N0}$)"
                : $"-{actualLoss:N0}$\n(Loss Reduction {savedAmount:N0}$)";
        else
            msg = $"-{actualLoss:N0}$";

        bettingMessageText.text = msg;
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