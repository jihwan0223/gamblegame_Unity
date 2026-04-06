using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public PlayerHand playerHand;
    public PlayerHand dealerHand;
    public DealerAI dealerAI;
    public DeckManager deckManager; // 덱을 다시 섞거나 관리할 경우

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
    // 버튼들이 인스펙터에서 연결되었는지 확인하며 실행
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
        // 1. 카드 오브젝트 삭제 및 데이터 리셋 (이미 만들어둔 ClearHand 활용)
        playerHand.ClearHand();
        dealerHand.ClearHand();

        // 2. 덱이 부족하다면 새로 섞기 (필요 시)
        // deckManager.Reshuffle(); 

        // 3. UI 상태 복구
        dealerAI.speechText.text = "Place your bets! New game started.";
        hitButton.interactable = true;
        stayButton.interactable = true;
        nextGameButton.gameObject.SetActive(false);

        // 4. 초기 카드 2장씩 드로우 (블랙잭 기본 규칙)
        StartInitialDraw();
    }

    private void StartInitialDraw()
    {
        // 처음에 각각 2장씩 뽑아주는 로직
        playerHand.AddCard(deckManager.DrawCard());
        dealerHand.AddCard(deckManager.DrawCard());
        playerHand.AddCard(deckManager.DrawCard());
        dealerHand.AddCard(deckManager.DrawCard());
        
        Debug.Log("새 라운드가 시작되었습니다!</color>");
    }
}