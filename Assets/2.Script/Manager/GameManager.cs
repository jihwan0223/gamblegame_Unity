using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public PlayerHand playerHand;
    public PlayerHand dealerHand;
    public DealerAI dealerAI;
    public DeckManager deckManager;

    [Header("UI Buttons")]
    public Button hitButton;
    public Button stayButton;

    [Header("Bet UI")]
    public TMP_InputField bettingInput;
    public BettingManager bettingManager;

    [Header("게임 시작시 나타나는 버튼")]
    public Button upgradeButton;
    public Button lobbyButton;

    [Header("게임 종료 후 나타나는 버튼")]
    public Button nextGameButton;
    

    private void Start()
    {
        // 버튼 리스너 연결
        nextGameButton.onClick.AddListener(ResetBoardForNextRound);
        upgradeButton.gameObject.SetActive(true);
        lobbyButton.gameObject.SetActive(true);
        bettingInput.gameObject.SetActive(true);

        nextGameButton.gameObject.SetActive(false);
        hitButton.interactable = false;
        stayButton.interactable = false;   
    }

    public void StartGame()
    {
        bettingInput.gameObject.SetActive(false);
        lobbyButton.gameObject.SetActive(false);
        upgradeButton.gameObject.SetActive(false);

        hitButton.interactable = true;
        stayButton.interactable = true;

        StartDealerHand();
    }

    private void StartDealerHand()
    {
        dealerHand.AddCard(deckManager.DrawCard());
        dealerHand.AddCard(deckManager.DrawCard());
    }

    // 게임이 완전히 끝났을 때 DealerAI 등에서 호출해줄 함수
    public void OnGameEnd()
    {
        if (hitButton != null) hitButton.interactable = false;
        if (stayButton != null) stayButton.interactable = false;
        if (nextGameButton != null) nextGameButton.gameObject.SetActive(true);
        if (upgradeButton != null) upgradeButton.gameObject.SetActive(true);
        if (lobbyButton != null) lobbyButton.gameObject.SetActive(true);
        else Debug.LogError("GameManager: Next Game Button이 연결되지 않았습니다!");
    }

    
    public void ResetBoardForNextRound()
    {
        // 플레이어와 딜러의 카드 삭제
        playerHand.ClearHand();
        dealerHand.ClearHand();

        // 덱 초기화
        if (deckManager != null)
        {
            deckManager.ResetDeck();
        }

        // UI 및 버튼 상태 복구
        bettingManager.ResetBet();

        bettingInput.text = "";
        bettingInput.gameObject.SetActive(true);
        lobbyButton.gameObject.SetActive(true);
        upgradeButton.gameObject.SetActive(true);

        nextGameButton.gameObject.SetActive(false);

        hitButton.interactable = false;
        stayButton.interactable = false;

        dealerAI.speechText.text = "Next Turn, Good Luck!";
        dealerAI.LWinText.text = "";
        
        Debug.Log("<color=orange>판이 정리되고 덱이 초기화되었습니다.</color>");
    }
}