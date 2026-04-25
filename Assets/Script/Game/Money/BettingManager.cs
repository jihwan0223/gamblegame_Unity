using UnityEngine;

public class BettingManager : MonoBehaviour
{
    // 승리시 금액 계산
    public void OnGameWin(long betAmount, float winPercent)
    {
        if (DataManager.instance == null)
        {
            Debug.LogError("DataManager 없음");
            return;
        }

        long profit = (long)(betAmount * (winPercent / 100f));
        DataManager.instance.gameData.money += profit;

        Debug.Log($"승리! 이익: {profit}, 현재 잔액: {DataManager.instance.gameData.money}");
        DataManager.instance.SaveGameData();
        SoundManager.instance.Play("Money");
    }

    // 패배시 금액 계산
    public void OnGameLose(long betAmount, float losePercent)
    {
        long loss = (long)(betAmount * (losePercent / 100f));
        DataManager.instance.gameData.money -= loss;

        // 돈 < 0
        if (DataManager.instance.gameData.money < 0)
        {
            DataManager.instance.gameData.money = 0;
        }

        Debug.Log($"패배.. 손실: {loss}, 현재 잔액: {DataManager.instance.gameData.money}");
        DataManager.instance.SaveGameData(); // 저장
    }
}
