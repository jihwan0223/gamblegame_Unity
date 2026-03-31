using UnityEngine;
using System.Collections.Generic;

public class DeckManager : MonoBehaviour
{
    private List<Card> _deck = new List<Card>();
    
    void Awake()
    {
        GenerateDeck();
        Shuffle();
    }

    private void GenerateDeck()
    {
        _deck.Clear();

        for (int s = 0; s < 4; s++)
        {
            for (int r = 1; r <= 13; r++)
            {

                _deck.Add(new Card((Card.CardSuit)s, r));
            }
        }
    }

    // 카드 섞기
    public void Shuffle()
    {
        for (int i = 0; i < _deck.Count; i++)
        {
            int randomIndex = Random.Range(i, _deck.Count);
            Card temp = _deck[i];
            _deck[i] = _deck[randomIndex];
            _deck[randomIndex] = temp;
        }
    }

    // 카드 뽑기
    public Card DrawCard()
    {
        if (_deck.Count <= 0)
        {
            return null;
        }

        Card drawnCard = _deck[0];
        _deck.RemoveAt(0);
        return drawnCard;
    }
}