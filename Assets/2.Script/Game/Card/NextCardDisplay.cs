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
        HideButton();
    }

    // 스킬 해금 시 외부에서 호출
    public void ShowButton()
    {
        if (nextCardButton != null) nextCardButton.SetActive(true);
    }

    public void HideButton()
    {
        if (nextCardButton != null) nextCardButton.SetActive(false);
    }

    // 배팅 완료 후 스킬 해금 여부 체크
    public void CheckAndShowButton()
    {
        if (DataManager.instance.gameData.skillLevels[6] > 0)
            ShowButton();
    }

    // 확인 버튼 OnClick()에 연결
    public void RevealNextCard()
    {
        if (deckManager == null || nextCardText == null) return;

        Card next = deckManager.PeekNextCard();
        if (next == null) return;

        int displayScore  = next.rank >= 11 ? 10 : next.rank;
        string suitSymbol = GetSuitSymbol(next.suit);

        nextCardText.text = LanguageToggle.Instance._isKorean
            ? $"다음 카드 : {suitSymbol} {displayScore}"
            : $"Next Card : {suitSymbol} {displayScore}";

        _isRevealed = true;
        HideButton();
    }

    // 플레이어 카드 뽑으면 호출
    public void HideNextCard()
    {
        nextCardText.text = "";
        _isRevealed       = false;
    }

    // 게임 리셋 시 호출
    public void Reset()
    {
        HideNextCard();
        HideButton();
    }

    private string GetSuitSymbol(Card.CardSuit suit)
    {
        return suit switch
        {
            Card.CardSuit.Heart   => "♥",
            Card.CardSuit.Diamond => "♦",
            Card.CardSuit.Club    => "♣",
            Card.CardSuit.Spade   => "♠",
            _ => ""
        };
    }
}