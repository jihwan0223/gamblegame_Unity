using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameManager gameManager;

    [Header("Manager References")]
    public DeckManager deckManager;
    public DealerAI dealerAI;
    public PlayerHand playerHand;
    public BettingManager bettingManager;

    [Header("UI Button")]
    public Button hitButton;
    public Button stayButton; // 배팅을 멈추고 게임을 시작하는 버튼

    // 인게임 플레이 중에만 버튼들을 끄고 켤 때 사용
    public void SetButtonsInteractable(bool value)
    {
        if (hitButton != null)  hitButton.interactable  = value;
        if (stayButton != null) stayButton.interactable = value;
    }

    public void OnClickHit()
    {
        Card newCard = deckManager.DrawCard();
        if (newCard != null)
        {
            SoundManager.instance.OnClickButton();
            playerHand.AddCard(newCard);

            if (playerHand.GetTotalScore() > 21)
                OnClickStay();

            Card nextCard = deckManager.PeekNextCard();
            Debug.Log(nextCard != null
                ? $"다음 카드 : {nextCard.rank}"
                : "덱이 비어있어 다음 카드가 없습니다.");
        }
    }

    // [수정] 배팅 완료 후 딜러 턴으로 넘어가는 Stay 버튼 기능
    public void OnClickStay()
    {
        // 1. 배팅이 아직 안 끝났다면 배팅을 먼저 확정(돈 차감)시킵니다.
        if (bettingManager != null && !bettingManager.IsBetDone())
        {
            // 배팅 금액이 0원 이하이면 시작 못 하게 막음
            if (bettingManager.GetPendingBet() <= 0)
            {
                bettingManager.ShowMessage("Please select a bet amount!");
                return;
            }
            
            // 배팅 확정 (여기서 돈이 정상적으로 깎입니다)
            bettingManager.OnClickConfirmBet(); 
        }

        // 2. 인게임 버튼들 비활성화
        SetButtonsInteractable(false);
        
        // 3. 배팅 UI 숨기고 딜러 차례 시작
        if (bettingManager != null) bettingManager.HideBettingUI();
        StartCoroutine(dealerAI.PlayTurn(playerHand.GetTotalScore()));
    }
}