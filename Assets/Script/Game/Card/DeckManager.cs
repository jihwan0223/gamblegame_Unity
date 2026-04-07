using UnityEngine;
using System.Collections.Generic;

public class DeckManager : MonoBehaviour
{
    private List<Card> _deck = new List<Card>();
    
    void Awake()
    {
        // 덱 생성 및 섞기
        ResetDeck();
    }
    
    public void ResetDeck()
    {
        GenerateDeck();
        Shuffle();
        Debug.Log("<color=cyan>[Deck]</color> 덱이 새로 생성되고 섞였습니다.");
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

    // 카드 섞기 (Fisher-Yates 알고리즘)
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
            Debug.LogWarning("덱에 카드가 없습니다! 자동으로 리셋합니다.");
            ResetDeck(); // 카드가 없으면 자동으로 다시 생성
        }

        Card drawnCard = _deck[0];
        _deck.RemoveAt(0);
        return drawnCard;
    }
}