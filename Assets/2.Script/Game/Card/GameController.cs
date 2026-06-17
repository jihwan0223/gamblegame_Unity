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
    public Button stayButton;

    public void SetButtonsInteractable(bool value)
    {
        if (hitButton != null)  hitButton.interactable  = value;
        if (stayButton != null) stayButton.interactable = value;
    }

    // Stay 버튼 - 배팅 확정 + 게임 시작
    public void OnClickStay()
    {
        hitButton.interactable  = false;
        stayButton.interactable = false;
        if (stayButton != null) stayButton.gameObject.SetActive(false);
        bettingManager.HideBettingUI();
        StartCoroutine(dealerAI.PlayTurn(playerHand.GetTotalScore()));
    }

    // Hit 버튼 - 게임 중 카드 뽑기
    public void OnClickHit()
    {
        Card newCard = deckManager.DrawCard();
        if (newCard == null) return;

        SoundManager.instance.OnClickButton();
        playerHand.AddCard(newCard);

        if (playerHand.GetTotalScore() > 21)
        {
            hitButton.interactable  = false;
            stayButton.interactable = false;
            bettingManager.HideBettingUI();
            StartCoroutine(dealerAI.PlayTurn(playerHand.GetTotalScore()));
        }
    }
}
