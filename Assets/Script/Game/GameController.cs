using UnityEngine;

public class GameController : MonoBehaviour
{
    [Header("DeckManager")]
    public DeckManager deckManager;

    public PlayerHand playerHand;


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
}
