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
    public TextMeshProUGUI speechText;      // 딜러 메시지
    public TextMeshProUGUI LWinText;        // 승패 text
    public string scorePrefix = "Score: ";

    [Header("Manager")]
    public BettingManager bettingManager;
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
        SoundManager.instance.Play("CardDrawSound");

        // 블랙잭
        if (playerTotalScore == 21)
        {
            Debug.Log("<color=cyan>[결과] 플레이어 블랙잭.</color>");
            LWinText.text = "BlackJack!!";

            bettingManager.OnGameWin(200f);

            RequestAIDecision(playerTotalScore, dealerHand.GetTotalScore());
            yield break;
        }
        
        // 플레이어 버스트
        if (playerTotalScore > 21)
        {
            Debug.Log($"<color=red>플레이어 버스트{playerTotalScore}</color>");
            LWinText.text = "Bust!";

            bettingManager.OnGameLose();

            RequestAIDecision(playerTotalScore, dealerHand.GetTotalScore());
            yield break;
        }

        // ⭐ 딜러 카드 뽑기
        while (dealerHand.GetTotalScore() < 17)
        {
            yield return new WaitForSeconds(1.0f);
            dealerHand.AddCard(deckManager.DrawCard());
        }

        int dealerTotalScore = dealerHand.GetTotalScore();

        // ⭐ 결과 판정 (여기서만 실행됨)
        if (dealerTotalScore > 21)
        {
            // 딜러 버스트 → 플레이어 승
            Debug.Log($"<color=green>[결과] 딜러 버스트({dealerTotalScore})! 플레이어 승리.</color>");
            LWinText.text = "You Win!";
            bettingManager.OnGameWin(100f);
        }
        else if (playerTotalScore > dealerTotalScore)
        {
            // 플레이어 승
            Debug.Log($"<color=green>[결과] 플레이어({playerTotalScore}) 승리! (딜러: {dealerTotalScore})</color>");
            LWinText.text = "You Win!";
            bettingManager.OnGameWin(100f);
        }
        else if (playerTotalScore < dealerTotalScore)
        {
            // 딜러 승
            Debug.Log($"<color=red>[결과] 딜러({dealerTotalScore}) 승리! (플레이어: {playerTotalScore})</color>");
            LWinText.text = "You Lose...";
            bettingManager.OnGameLose();
        }
        else
        {
            // 무승부
            Debug.Log($"<color=white>[결과] {playerTotalScore}점으로 무승부(Push)입니다.</color>");
            LWinText.text = "Draw!";
            bettingManager.OnGameDraw(); // ⭐ 추가
        }

        // AI 응답
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