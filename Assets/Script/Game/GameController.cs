using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [Header("Manager References")]
    public DeckManager deckManager;
    public DealerAI dealerAI;      // ★ 추가: 딜러 AI 연결 필요
    public PlayerHand playerHand;
    public PlayerHand dealerHand;  // ★ 추가: 딜러 핸드 연결 필요

    [Header("UI Button")]
    public Button hitButton;
    public Button stayButton;

    public void OnClickHit()
    {
        Card newCard = deckManager.DrawCard();
        if (newCard != null) playerHand.AddCard(newCard);
        else Debug.Log("덱이 비어있음");
    }

    public void OnClickStay()
    {
        Debug.Log("Stay 버튼 클릭됨");

        if (hitButton != null) hitButton.interactable = false;
        if (stayButton != null) stayButton.interactable = false;

        StartDealerTurn();
    }

    private void StartDealerTurn()
    {
        Debug.Log("딜러 턴 시작 명령 보냄");
        
        // ★ 핵심: DealerAI에 있는 코루틴을 실행시켜야 합니다!
        if (dealerAI != null)
        {
            // 플레이어 점수를 넘겨주며 딜러의 행동을 시작시킵니다.
            StartCoroutine(dealerAI.PlayTurn(playerHand.GetTotalScore()));
        }
        else
        {
            Debug.LogError("DealerAI가 GameController에 연결되지 않았습니다!");
        }
    }
    
    // 딜러 코루틴이 끝날 때 호출될 함수 (승패 판정)
    public void DetermineWinner()
    {
        int pScore = playerHand.GetTotalScore();
        int dScore = dealerHand.GetTotalScore();

        Debug.Log($"최종 점수 - 플레이어: {pScore}, 딜러: {dScore}");

        if (pScore > 21) Debug.Log("플레이어 버스트 패배!");
        else if (dScore > 21 || pScore > dScore) Debug.Log("플레이어 승리!");
        else if (dScore > pScore) Debug.Log("딜러 승리!");
        else Debug.Log("무승부!");
    }
}