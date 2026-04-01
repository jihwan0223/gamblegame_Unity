using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [Header("DeckManager")]
    public DeckManager deckManager;

    public PlayerHand playerHand;

    [Header("UI Button")]
    public Button hitButton;
    public Button stayButton;


    public void OnClickHit()
    {
        Card newCard = deckManager.DrawCard();

        if (newCard != null)
        {
            playerHand.AddCard(newCard);
        }

        else
        {
            Debug.Log("덱이 비어있음");
        }
    }

    public void OnClickStay()
    {
        Debug.Log("Stay");

        if (hitButton != null) hitButton.interactable = false;
        if (stayButton != null) stayButton.interactable = false;

        StartDealerTurn();
    }

    private void StartDealerTurn()
    {
        Debug.Log("딜러가 카드를 뽑음");
    }
}
