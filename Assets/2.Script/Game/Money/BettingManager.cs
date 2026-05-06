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

    // 배팅 버튼
    public void OnClickBet()
    {
        if(isBetDone) return;

        if(string.IsNullOrEmpty(inputField.text)) return;

        if (!long.TryParse(inputField.text, out long bet))
        {
            bettingMessageText.text = "Enter betting amount";
            return;
        }

        if (bet <= 0 || bet > DataManager.instance.gameData.money)
        {
            bettingMessageText.text = "Please enter the correct betting amount..!";
            return;
        }

        currentBet = bet;
        DataManager.instance.gameData.money -= currentBet;
        DataManager.instance.SaveGameData();

        Debug.Log("배팅 완료 : " + currentBet);
        currentBettingText.text = $"betting : {currentBet}";

        isBetDone = true;

        gameManager.StartGame();
    }

    // 승리
    public void OnGameWin(float percent)
    {
        if (currentBet <= 0) return;

        long profit = (long)(currentBet * (percent / 100f));
        long reward = currentBet + profit;

        DataManager.instance.gameData.money += reward;
        DataManager.instance.SaveGameData();

        bettingMessageText.text = $"+{reward}$";

        SoundManager.instance.PlayerWin();

        ResetBet();
    }

    // 패배
    public void OnGameLose()
    {
        if(currentBet <= 0) return;

        bettingMessageText.text = $"-{currentBet}$";

        SoundManager.instance.PlayerLose();

        ResetBet();
    }

    // 무승부
    public void OnGameDraw()
    {
        if(currentBet <= 0) return;

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