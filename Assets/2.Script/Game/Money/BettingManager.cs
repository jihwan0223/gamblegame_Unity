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
    public NextCardDisplay nextCardDisplay;

    [Header("배팅 패널 토글 버튼")]
    public GameObject bettingToggleButton;

    [Header("Betting UI")]
    public GameObject bettingButtonsPanel;
    public GameObject confirmBetButton;

    private long currentBet  = 0;
    private bool isBetDone   = false;
    private long _pendingBet = 0;
    private bool _isAllIn    = false;

    public bool IsBetDone()      => isBetDone;
    public long GetPendingBet()  => _pendingBet;

    public void ShowMessage(string text) => bettingMessageText.text = text;

    // +버튼
    public void AddBet(long amount)
    {
        if (isBetDone) return;
        long money  = DataManager.instance.gameData.money;
        long newBet = (long)Mathf.Min(_pendingBet + amount, money);
        _pendingBet = newBet;
        UpdateBetUI();
    }

    public void AddBet1000()  => AddBet(1000);
    public void AddBet10000() => AddBet(10000);

    // -버튼
    private void SubBet(long amount)
    {
        if (isBetDone) return;
        _pendingBet = (long)Mathf.Max(0, _pendingBet - amount);
        UpdateBetUI();
    }

    public void SubBet10()    => SubBet(10);
    public void SubBet100()   => SubBet(100);
    public void SubBet1000()  => SubBet(1000);
    public void SubBet10000() => SubBet(10000);

    // 절반
    public void HalfBet()
    {
        if (isBetDone) return;
        _pendingBet = DataManager.instance.gameData.money / 2;
        UpdateBetUI();
    }

    // 올인
    public void OnClickAllIn()
    {
        if (isBetDone) return;
        _pendingBet = DataManager.instance.gameData.money;
        UpdateBetUI();
    }

    // 클리어
    public void ClearBet()
    {
        if (isBetDone) return;
        _pendingBet = 0;
        UpdateBetUI();
    }

    public void CloseBettingPanel()
    {
        if (bettingButtonsPanel != null) bettingButtonsPanel.SetActive(false);
        if (confirmBetButton != null)    confirmBetButton.SetActive(false);
    }

    // 배팅 패널 토글
    public void ToggleBettingPanel()
    {
        if (isBetDone) return;
        bool isActive = bettingButtonsPanel.activeSelf;
        bettingButtonsPanel.SetActive(!isActive);
        if (confirmBetButton != null)
            confirmBetButton.SetActive(!isActive);
    }

    public void ShowBettingUI()
    {
        if (bettingButtonsPanel != null) bettingButtonsPanel.SetActive(true);
        if (confirmBetButton != null)    confirmBetButton.SetActive(true);
    }

    public void HideBettingUI()
    {
        if (bettingButtonsPanel != null) bettingButtonsPanel.SetActive(false);
        if (confirmBetButton != null)    confirmBetButton.SetActive(false);
    }

    public void ShowToggleButton()
    {
        if (bettingToggleButton != null) bettingToggleButton.SetActive(true);
    }

    public void HideToggleButton()
    {
        if (bettingToggleButton != null) bettingToggleButton.SetActive(false);
    }

    // 배팅 패널 닫기 버튼
    public void OnClickConfirmBet()
    {
        if (_pendingBet <= 0)
        {
            bettingMessageText.text = LanguageToggle.Instance._isKorean
                ? "베팅 금액을 선택하세요!" : "Please select a bet amount!";
            return;
        }

        long money = DataManager.instance.gameData.money;
        _isAllIn   = _pendingBet == money;
        currentBet = _pendingBet;

        DataManager.instance.gameData.money -= currentBet;
        DataManager.instance.SaveGameData();

        currentBettingText.text = _isAllIn
            ? (LanguageToggle.Instance._isKorean ? $"올인!! : {currentBet:N0}" : $"ALL IN!! : {currentBet:N0}")
            : (LanguageToggle.Instance._isKorean ? $"베팅 : {currentBet:N0}" : $"Betting : {currentBet:N0}");

        isBetDone   = true;
        _pendingBet = 0;

        bettingButtonsPanel.SetActive(false);
        if (confirmBetButton != null) confirmBetButton.SetActive(false);
        HideToggleButton();

        gameManager.StartGame();
        gameManager.gameController.stayButton.gameObject.SetActive(true);
        gameManager.gameController.hitButton.interactable  = true;
        gameManager.gameController.stayButton.interactable = true;

        nextCardDisplay.CheckAndShowButton();
    }

    // GameController에서 호출 - 돈 차감만 (StartGame 없음)
    public void ConfirmBetOnly()
    {
        long money = DataManager.instance.gameData.money;
        if (_pendingBet <= 0 || _pendingBet > money) return;

        _isAllIn   = _pendingBet == money;
        currentBet = _pendingBet;

        DataManager.instance.gameData.money -= currentBet;
        DataManager.instance.SaveGameData();

        currentBettingText.text = _isAllIn
            ? (LanguageToggle.Instance._isKorean ? $"올인!! : {currentBet:N0}" : $"ALL IN!! : {currentBet:N0}")
            : (LanguageToggle.Instance._isKorean ? $"베팅 : {currentBet:N0}" : $"Betting : {currentBet:N0}");

        isBetDone   = true;
        _pendingBet = 0;

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
        DataManager.instance.gameData.money = System.Math.Min(DataManager.instance.gameData.money, long.MaxValue - 1);

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
        DataManager.instance.gameData.money = System.Math.Min(DataManager.instance.gameData.money, long.MaxValue - 1);

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
        DataManager.instance.gameData.money = System.Math.Min(DataManager.instance.gameData.money, long.MaxValue - 1);

        DataManager.instance.SaveGameData();

        bettingMessageText.text = $"+{0}$";
        ResetBet();
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