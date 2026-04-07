using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public PlayerHand playerHand;
    public PlayerHand dealerHand;
    public DealerAI dealerAI;
    public DeckManager deckManager;

    [Header("UI Buttons")]
    public Button hitButton;
    public Button stayButton;
    public Button nextGameButton; // 게임 종료 후 나타날 "Next Round" 버튼

    private void Start()
    {
        // 버튼 리스너 연결
        nextGameButton.onClick.AddListener(ResetBoardForNextRound);
        nextGameButton.gameObject.SetActive(false);
    }

    // 게임이 완전히 끝났을 때 DealerAI 등에서 호출해줄 함수
    public void OnGameEnd()
    {
        
    if (hitButton != null) hitButton.interactable = false;
    if (stayButton != null) stayButton.interactable = false;
    
    if (nextGameButton != null)
    {
        nextGameButton.gameObject.SetActive(true);
    }
    else
    {
        Debug.LogError("GameManager: Next Game Button이 연결되지 않았습니다!");
        }
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
    hitButton.interactable = true;
    stayButton.interactable = true;
    nextGameButton.gameObject.SetActive(false);
    dealerAI.speechText.text = "Deck reshuffled. New game!";

    Debug.Log("<color=orange>판이 정리되고 덱이 초기화되었습니다.</color>");
    }
}