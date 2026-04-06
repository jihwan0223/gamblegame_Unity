using UnityEngine;
using System.Collections;
using TMPro;

public class DealerAI : MonoBehaviour
{
    [Header("Dependencies")]
    public GroqAI groqAI;
    public DeckManager deckManager;
    public PlayerHand dealerHand;

    [Header("UI")]
    public TextMeshProUGUI speechText;

    private GameManager gameManager;

    private void Start()
    {
        gameManager = Object.FindFirstObjectByType<GameManager>(); 
    }

    public IEnumerator PlayTurn(int playerTotalScore)
    {
        speechText.text = "Dealer's turn...";
        yield return new WaitForSeconds(0.5f);

        // 1. 플레이어가 21점 미만일 때만 딜러가 17점까지 카드를 뽑습니다.
        if (playerTotalScore < 21)
        {
            while (dealerHand.GetTotalScore() < 17)
            {
                yield return new WaitForSeconds(1.0f);
                dealerHand.AddCard(deckManager.DrawCard());
                Debug.Log($"딜러가 카드를 뽑았습니다. 현재 점수: {dealerHand.GetTotalScore()}");
            }
        }
        // 2. 플레이어가 21점이면 즉시 중단
        else if (playerTotalScore == 21)
        {
            Debug.Log("플레이어 21점! 딜러가 즉시 멈춥니다.");
            speechText.text = "Impressive score...";
            yield return new WaitForSeconds(0.5f);
        }

        // --- [승패 판정 Debug.Log 추가] ---
        int dealerTotalScore = dealerHand.GetTotalScore();
        if (playerTotalScore < dealerTotalScore)
        {
            Debug.Log($"<color=Red>[결과] 딜러({dealerTotalScore}) 승리! (플레이어: {playerTotalScore})</color>");
        }
        else if (playerTotalScore > 21)
        {
            Debug.Log("<color=Red>[결과] 플레이어 버스트! 딜러 승리.</color>");
        }
        
        else if (playerTotalScore > dealerTotalScore)
        {
            Debug.Log($"<color=green>[결과] 플레이어({playerTotalScore}) 승리! (딜러: {dealerTotalScore})</color>");
        }
        else if (dealerTotalScore > 21)
        {
            Debug.Log("<color=Green>[결과] 딜러 버스트! 플레이어 승리.</color>");
        }
        
        
        else
        {
            Debug.Log("<color=white>[결과] 무승부(Push)입니다.</color>");
        }
        // ----------------------------------

        // 3. 모든 상황 종료 후 AI에게 결과 판단 요청 (영어 대사 출력)
        RequestAIDecision(playerTotalScore, dealerTotalScore);
    }

    private void RequestAIDecision(int pScore, int dScore)
    {
        speechText.text = "..."; 

        groqAI.GetDealerResponse(pScore, dScore, (response) => {
            speechText.text = response; 
            
            // 저장해둔 매니저 호출 (안전하게 null 체크 추가)
            if (gameManager != null)
            {
                gameManager.OnGameEnd();
            }
        });
    }
}