using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [Header("Manager References")]
    public DeckManager deckManager;
    public DealerAI dealerAI;
    public PlayerHand playerHand;
    public BettingManager bettingManager;

    [Header("UI Button")]
    public Button hitButton;
    public Button stayButton;

    public void OnClickHit()
    {
        Card newCard = deckManager.DrawCard();
        if (newCard != null) 
        {
            SoundManager.instance.OnClickButton();

            playerHand.AddCard(newCard);

            if (playerHand.GetTotalScore() > 21) 
                OnClickStay();

            // 다음 카드
            Card nextCard = deckManager.PeekNextCard();
            if (nextCard != null)
            {
                Debug.Log($"다음 카드 : {nextCard.rank}");
            }
            else
            {
                Debug.Log("덱이 비어있어 다음 카드가 없습니다.");
            }
        }
    }

    public void OnClickStay()
    {
        if (hitButton != null) hitButton.interactable = false;
        if (stayButton != null) stayButton.interactable = false;

        Debug.Log("플레이어 Stay.");
        // 딜러 AI에게 플레이어 점수를 넘겨주며 시작
        StartCoroutine(dealerAI.PlayTurn(playerHand.GetTotalScore()));
    }
}