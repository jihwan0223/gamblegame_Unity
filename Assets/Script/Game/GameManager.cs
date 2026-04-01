using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("연결할 오브젝트들")]
    public DealerAI dealerAI;
    public PlayerHand playerHand;
    public PlayerHand dealerHand;

    [Header("게임 상태")]
    public int currentRound = 1;

    void Start()
    {
        // 게임 시작
        StartNextRound();
    }

    public void StartNextRound()
    {
        // 모든 라운드 클리어
        if (currentRound > 3)
        {
            Debug.Log("<color=cyan>게임 종료: 모든 라운드를 클리어하셨습니다!</color>");
            return;
        }

        if (dealerAI != null)
        {
            dealerAI.SetDifficulty(currentRound);
            Debug.Log($"라운드: {currentRound} / 난이도 {dealerAI.currentDifficulty}");
        }
    }

    // 승패 판정
    public void DetermineWinner()
    {
        int pScore = playerHand.GetTotalScore();
        int dScore = dealerHand.GetTotalScore();

        if (pScore > 21)
        {
            dealerAI.Speak("Bust_Player", pScore, dScore);
            Debug.Log("<color=red>플레이어 버스트 패배</color>");
        }
        else if (dScore > 21 || pScore > dScore)
        {
            Debug.Log("<color=green>플레이어 승리!</color>");
        }
        else if (dScore > pScore)
        {
            dealerAI.Speak("Dealer_Win", pScore, dScore);
            Debug.Log("<color=red>딜러 승리</color>");
        }
        else
        {
            Debug.Log("<color=white>무승부 (Push)</color>");
        }

        // 3초 후 다음 라운드 준비
        Invoke("OnRoundEnd", 3f);
    }

    // 라운드 종료 및 다음 라운드 준비
    public void OnRoundEnd()
    {
        currentRound++;

        // 카드 및 데이터 초기화
        if (playerHand != null) playerHand.ClearHand();
        if (dealerHand != null) dealerHand.ClearHand();

        StartNextRound();
    }
}