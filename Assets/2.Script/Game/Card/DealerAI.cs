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
        if (LanguageToggle.Instance._isKorean) speechText.text = "딜러의 차례...";
        else speechText.text = "Dealer's turn...";
        yield return new WaitForSeconds(1.0f);

        dealerHand.RevealAllCards();
        SoundManager.instance.DrawCard();

        // 블랙잭
        if (playerTotalScore == 21)
        {
            Debug.Log("<color=cyan>[결과] 플레이어 블랙잭.</color>");
            if (LanguageToggle.Instance._isKorean) LWinText.text = "블랙잭!!";
            else LWinText.text = "BlackJack!!";

            bettingManager.OnGameWin(200f);

            RequestAIDecision(playerTotalScore, dealerHand.GetTotalScore());
            yield break;
        }
        
        // 플레이어 버스트
        if (playerTotalScore > 21)
        {
            Debug.Log($"<color=red>플레이어 버스트{playerTotalScore}</color>");
            if (LanguageToggle.Instance._isKorean) LWinText.text = "버스트!";
            else LWinText.text = "Bust!";
            bettingManager.OnGameLose();

            RequestAIDecision(playerTotalScore, dealerHand.GetTotalScore());
            yield break;
        }

        // 딜러 카드 뽑기
        int dealerTotalScore = dealerHand.GetTotalScore();

        while (true)
        {
            dealerTotalScore = dealerHand.GetTotalScore();

            // 17 이상이면 멈춤
            if (dealerTotalScore >= 17)
                break;

            // 플레이어보다 높으면 멈춤
            if (dealerTotalScore > playerTotalScore)
                break;

            yield return new WaitForSeconds(1.0f);

            dealerHand.AddCard(deckManager.DrawCard());
}

        // 결과 판정 (여기서만 실행됨)
        if (dealerTotalScore > 21)
        {
            // 딜러 버스트 → 플레이어 승
            Debug.Log($"<color=green>[결과] 딜러 버스트({dealerTotalScore})! 플레이어 승리.</color>");
            if (LanguageToggle.Instance._isKorean) LWinText.text = "승리!";
            else LWinText.text = "You Win!";
            bettingManager.OnGameWin(100f);
        }
        else if (playerTotalScore > dealerTotalScore)
        {
            // 플레이어 승
            Debug.Log($"<color=green>[결과] 플레이어({playerTotalScore}) 승리! (딜러: {dealerTotalScore})</color>");
            if (LanguageToggle.Instance._isKorean) LWinText.text = "승리!";
            else LWinText.text = "You Win!";
            bettingManager.OnGameWin(100f);
        }
        else if (playerTotalScore < dealerTotalScore)
        {
            // 딜러 승
            Debug.Log($"<color=red>[결과] 딜러({dealerTotalScore}) 승리! (플레이어: {playerTotalScore})</color>");
            if (LanguageToggle.Instance._isKorean) LWinText.text = "패배...";
            else LWinText.text = "You Lose...";
            bettingManager.OnGameLose();
        }
        else
        {
            // 무승부
            Debug.Log($"<color=white>[결과] {playerTotalScore}점으로 무승부(Push)입니다.</color>");
            if (LanguageToggle.Instance._isKorean) LWinText.text = "무승부!";
            else LWinText.text = "Draw!";
            bettingManager.OnGameDraw();
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