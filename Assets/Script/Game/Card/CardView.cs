using UnityEngine;
using UnityEngine.UI;
public class CardView : MonoBehaviour
{
    private Image _cardImage;

    private void Awake()
    {
        _cardImage = GetComponent<Image>();
    }

    public void Setup(Card cardData)
    {
        string suitName = "";
        switch (cardData.suit)
        {
            case Card.CardSuit.Club:    suitName = "club"; break;
            case Card.CardSuit.Diamond: suitName = "diamond"; break;
            case Card.CardSuit.Heart:   suitName = "heart"; break;
            case Card.CardSuit.Spade:   suitName = "spade"; break;
        }

        // 숫자를 문자로 변환
        string rankString = cardData.rank.ToString();

        string fileName = $"{rankString}_{suitName}";

    // 4. 로드 및 적용
    Sprite cardSprite = Resources.Load<Sprite>(fileName);
    if (cardSprite != null && _cardImage != null)
    {
        _cardImage.sprite = cardSprite;
        _cardImage.color = Color.white; // 투명도 초기화
    }
    else
    {
        Debug.LogError($"파일을 찾을 수 없음: Resources/{fileName}");
    }
    }
}