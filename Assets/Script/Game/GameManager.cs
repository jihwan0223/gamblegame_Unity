using UnityEngine;

public class GameManager : MonoBehaviour
{
    public DealerAI dealerAI;
    public PlayerHand playerHand;
    public PlayerHand dealerHand;

    public void OnPlayerStay()
{
    Debug.Log("Stay 버튼 클릭됨");
    // 딜러가 카드를 뽑기 시작하게 명령
    StartCoroutine(dealerAI.PlayTurn(playerHand.GetTotalScore()));
}

    // 승패 판정
    public void DetermineWinner()
    {
        int pScore = playerHand.GetTotalScore();
        int dScore = dealerHand.GetTotalScore();

        if (pScore > 21) Debug.Log("<color=red>플레이어 버스트 패배!</color>");
        else if (dScore > 21) Debug.Log("<color=green>딜러 버스트! 플레이어 승리!</color>");
        else if (pScore > dScore) Debug.Log("<color=green>플레이어 승리!</color>");
        else if (dScore > pScore) Debug.Log("<color=red>딜러 승리!</color>");
        else Debug.Log("<color=white>무승부!</color>");

        // 필요하다면 여기서 Invoke("OnRoundEnd", 3f); 호출
    }
}