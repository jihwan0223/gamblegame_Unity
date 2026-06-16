using UnityEngine;
using TMPro;

public class BettingManager : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI bettingMessageText;
    public TextMeshProUGUI currentBettingText;

    [Header("Manager")]
    public GameManager gameManager;

    [Header("Next Card Button")]
    public GameObject nextCardButton;

    [Header("배팅 패널 토글 버튼")]
    public GameObject bettingToggleButton;

    // 외부에서 배팅 완료 여부를 확인할 수 있게 해주는 함수
    public bool IsBetDone() => isBetDone;

    // 외부에서 현재 누적된 배팅금을 확인할 수 있게 해주는 함수
    public long GetPendingBet() => _pendingBet;

    // 외부에서 에러 메시지를 띄울 수 있게 해주는 함수
    public void ShowMessage(string text)
    {
        bettingMessageText.text = text;
    }

    private long currentBet = 0;
    private bool isBetDone = false;
    private long _pendingBet = 0; // 누적 베팅 금액

    [Header("BettingButton")]
    private bool _isAllIn = false;
    // + 100
    public void AddBet1000() => AddBet(1000);
    public void AddBet10000() => AddBet(10000);

    [Header("Betting UI")]
    public GameObject bettingButtonsPanel; // +10, +100 등 버튼 묶음
    public GameObject confirmBetButton;    // 배팅 끝 버튼


    public void ClearBet()
    {
        if (isBetDone) return;
        _pendingBet = 0;
        UpdateBetUI();
    }

    public void ToggleBettingPanel()
    {    
        if (isBetDone) return;
        bool isActive = bettingButtonsPanel.activeSelf;
        bettingButtonsPanel.SetActive(!isActive);
        if (confirmBetButton != null)
            confirmBetButton.SetActive(!isActive);
    }

    // +버튼
    public void AddBet(long amount)
    {
        if (isBetDone) return;
        long money  = DataManager.instance.gameData.money;
        long newBet = (long)Mathf.Min(_pendingBet + amount, money);
        _pendingBet = newBet;
        UpdateBetUI();
    }

    // 절반
    public void HalfBet()
    {
        if (isBetDone) return;
        _pendingBet = DataManager.instance.gameData.money / 2;
        UpdateBetUI();
    }

    // 올인 (즉시 베팅)
    public void OnClickAllIn()
    {
        if (isBetDone) return;
        _pendingBet = DataManager.instance.gameData.money;
        UpdateBetUI();
    }

    // 베팅 확정 버튼
    public void OnClickBet()
    {
        if (isBetDone) return;
        if (_pendingBet <= 0)
        {
            bettingMessageText.text = LanguageToggle.Instance._isKorean
                ? "베팅 금액을 선택하세요!" : "Please select a bet amount!";
            return;
        }
        ConfirmBet();
    }

    public void ShowBettingUI()
    {
        bettingButtonsPanel.SetActive(true);
        confirmBetButton.SetActive(true);
    }

    public void OnClickConfirmBet()
    {
        if (_pendingBet <= 0)
        {
            bettingMessageText.text = LanguageToggle.Instance._isKorean
                ? "베팅 금액을 선택하세요!" : "Please select a bet amount!";
            return;
        }
        bettingButtonsPanel.SetActive(false);
        confirmBetButton.SetActive(false);
        if (bettingToggleButton != null) bettingToggleButton.SetActive(false);
        ConfirmBet();
    }

    public void HideBettingUI()
    {
        if (bettingButtonsPanel != null) bettingButtonsPanel.SetActive(false);
        if (confirmBetButton != null)    confirmBetButton.SetActive(false);
    }

    private void ConfirmBet()
    {
        Debug.Log($"ConfirmBet / money: {DataManager.instance.gameData.money} / pendingBet: {_pendingBet}");
        long money = DataManager.instance.gameData.money;

        if (_pendingBet <= 0 || _pendingBet > money)
        {
            bettingMessageText.text = LanguageToggle.Instance._isKorean
                ? "올바른 베팅 금액을 입력하세요!" : "Please enter the correct betting amount!";
            return;
        }

        _isAllIn   = _pendingBet == money;
        currentBet = _pendingBet;

        DataManager.instance.gameData.money -= currentBet;
        DataManager.instance.SaveGameData();

        currentBettingText.text = _isAllIn
            ? (LanguageToggle.Instance._isKorean ? $"올인!! : {currentBet:N0}" : $"ALL IN!! : {currentBet:N0}")
            : (LanguageToggle.Instance._isKorean ? $"베팅 : {currentBet:N0}" : $"Betting : {currentBet:N0}");

        isBetDone   = true;
        _pendingBet = 0;
        gameManager.StartGame();

        if (DataManager.instance.gameData.skillLevels[6] > 0)
            nextCardButton.SetActive(true);
    }

    private void UpdateBetUI()
    {
        currentBettingText.text = LanguageToggle.Instance._isKorean
            ? $"베팅 : {_pendingBet:N0}"
            : $"Betting : {_pendingBet:N0}";
    }

    // 승리
    public void OnGameWin(float percent)
    {
        if (currentBet <= 0) return;

        long profit         = (long)(currentBet * (percent / 100f));
        long reward         = currentBet + profit;
        long originalReward = reward;

        if (_isAllIn)
            reward = UpgradeManager.instance.CalcAllInBonus(reward);

        reward = UpgradeManager.instance.CalcWinReward(reward);
        long bonus = reward - originalReward;

        DataManager.instance.gameData.money += reward;
        DataManager.instance.SaveGameData();

        string msg = $"+{reward:N0}$";
        if (_isAllIn && bonus > 0)
            msg += LanguageToggle.Instance._isKorean
                ? $"\n(올인 보너스 +{bonus:N0}$)"
                : $"\n(All-In Bonus +{bonus:N0}$)";
        else if (bonus > 0)
            msg += LanguageToggle.Instance._isKorean
                ? $"\n(승리 보상 +{bonus:N0}$)"
                : $"\n(Win Boost +{bonus:N0}$)";

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

    public void ShowToggleButton()
    {
        if (bettingToggleButton != null) bettingToggleButton.SetActive(true);
    }
    
    public void ResetBet()
    {
        currentBet  = 0;
        _pendingBet = 0;
        _isAllIn    = false;
        isBetDone   = false;
        currentBettingText.text = "";
    }
}