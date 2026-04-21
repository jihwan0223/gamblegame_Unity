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
    public string scorePrefix = "Score: ";

    private GameManager gameManager;

    private void Start()
    {
        gameManager = Object.FindFirstObjectByType<GameManager>(); 
    }

    // 1. 딜러 카드 뽑기 로직 -----------------------------------------
    public IEnumerator PlayTurn(int playerTotalScore)
    {   
        speechText.text = "Dealer's turn...";
        yield return new WaitForSeconds(1.0f);
        dealerHand.RevealAllCards(); 

        if (playerTotalScore == 21)
        {
            Debug.Log("<color=cyan>[결과] 플레이어 21점 달성! 즉시 승리.</color>");

            int finalDealerScore = dealerHand.GetTotalScore();
            RequestAIDecision(playerTotalScore, finalDealerScore);
            yield break;
        }

        if (playerTotalScore <= 21)
        {
            // 17점 미만이면서 동시에 플레이어 점수보다 낮을 때만 뽑음
            while (dealerHand.GetTotalScore() < 17 && dealerHand.GetTotalScore() <= playerTotalScore)
            {
                yield return new WaitForSeconds(1.0f);
                dealerHand.AddCard(deckManager.DrawCard());
                
                if (dealerHand.GetTotalScore() > playerTotalScore) break; 
            }
        }
        else // 플레이어가 이미 21점을 넘은 경우
        {
            Debug.Log("플레이어 버스트!");
        }

        // 2. 최종 점수 계산
        int dealerTotalScore = dealerHand.GetTotalScore();

        // 3. 승패 판정 버스트 체크
        BettingManager betting = Object.FindFirstObjectByType<BettingManager>();
        long betAmount = 100;
        if (playerTotalScore > 21)
        {
            // 플레이어가 21을 넘으면 딜러 점수와 상관없이 무조건 플레이어 패배
            Debug.Log($"<color=red>[결과] 플레이어 버스트({playerTotalScore})! 딜러 승리.</color>");
            betting.OnGameLose(betAmount, 100f);    // 100% 잃음
        }

        else if (dealerTotalScore > 21)
        {
            // 딜러만 21을 넘은 경우
            Debug.Log($"<color=green>[결과] 딜러 버스트({dealerTotalScore})! 플레이어 승리.</color>");
            betting.OnGameWin(betAmount, 100f);
        }

        else if (playerTotalScore > dealerTotalScore)
        {
            // 둘 다 21 이하일 때 점수 비교
            Debug.Log($"<color=green>[결과] 플레이어({playerTotalScore}) 승리! (딜러: {dealerTotalScore})</color>");
            betting.OnGameWin(betAmount, 100f);
        }

        else if (playerTotalScore < dealerTotalScore)
        {
            // 둘 다 21 이하일 때 점수 비교
            Debug.Log($"<color=red>[결과] 딜러({dealerTotalScore}) 승리! (플레이어: {playerTotalScore})</color>");
            betting.OnGameLose(betAmount, 100f);
        }

        else
        {
            // 점수가 정확히 같을 때
            Debug.Log($"<color=white>[결과] {playerTotalScore}점으로 무승부(Push)입니다.</color>");
        }
            // ----------------------------------

            // 3. 모든 상황 종료 후 AI에게 결과 판단 요청
            RequestAIDecision(playerTotalScore, dealerTotalScore);
            }

    private void RequestAIDecision(int pScore, int dScore)
    {
        speechText.text = "..."; 

        groqAI.GetDealerResponse(pScore, dScore, (response) => {
            speechText.text = response; 
            
            if (gameManager != null)
            {
                gameManager.OnGameEnd();
            }
        });
    }
}