using TMPro;
using UnityEngine;

public class NextCardDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text nextCardText;
    [SerializeField] private DeckManager deckManager;
    [SerializeField] private GameObject nextCardButton;
    private bool _isRevealed = false;

    private void Start()
    {
        nextCardText.text = "";
    }

    // 확인 버튼 OnClick()에 연결
    public void RevealNextCard()
    {
        if (deckManager == null || nextCardText == null) return;

        Card next = deckManager.PeekNextCard();
        if (next == null) return;

        int displayScore  = next.rank >= 11 ? 10 : next.rank;
        string suitSymbol = GetSuitSymbol(next.suit);

        if (LanguageToggle.Instance._isKorean)
            nextCardText.text = $"다음 카드 : {suitSymbol} {displayScore}";
        else
            nextCardText.text = $"Next Card : {suitSymbol} {displayScore}";

        _isRevealed = true;
        nextCardButton.SetActive(false); // 버튼 숨기기
    }

    // 플레이어가 카드 뽑으면 호출
    public void HideNextCard()
    {
        nextCardText.text = "";
        _isRevealed = false;
    }

    private string GetSuitSymbol(Card.CardSuit suit)
    {
        return suit switch
        {
            Card.CardSuit.Heart => "♥",
            Card.CardSuit.Diamond => "♦",
            Card.CardSuit.Club => "♣",
            Card.CardSuit.Spade => "♠",
            _ => ""
        };
    }
}