using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [Header("Manager References")]
    public DeckManager deckManager;
    public DealerAI dealerAI;
    public PlayerHand playerHand;

    [Header("UI Button")]
    public Button hitButton;
    public Button stayButton;

    void Start()
    {
        if (SoundManager.instance != null)
        {
            Debug.Log("None");
        }
    }

    public void OnClickHit()
    {
        Card newCard = deckManager.DrawCard();
        if (newCard != null) {
            SoundManager.instance.PlaySFX(SoundManager.instance.cardDrow);
            playerHand.AddCard(newCard);
            // 플레이어가 뽑자마자 21 넘으면 바로 딜러 턴으로 넘김
            if (playerHand.GetTotalScore() > 21) OnClickStay();
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