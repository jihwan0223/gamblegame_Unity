using System;
using UnityEngine;
using UnityEngine.UI;
public class CardView : MonoBehaviour
{
    private Image _cardImage;
    private Sprite _frontSprite;
    [SerializeField] private string backCardFileName = "temp";      // 파일이름

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

        // 4. 로드 및 앞면 저장
        _frontSprite = Resources.Load<Sprite>(fileName);
    
        if (_frontSprite != null && _cardImage != null)
        {
            _cardImage.sprite = _frontSprite;
            _cardImage.color = Color.white;
        }
        else
        {
            Debug.LogError($"파일을 찾을 수 없음: Resources/{fileName}");
        }
    }

    // 뒷면
    public void SetFaceDown()
    {
        Sprite backSprite = Resources.Load<Sprite>(backCardFileName);
        if (backSprite != null && _cardImage != null)
        {
            _cardImage.sprite = backSprite;
        }
        else
        {
            Debug.LogError($"뒷면 이미지를 찾을 수 없습니다: Resources/{backCardFileName}");
        }
    }

    // 뒷면 -> 앞면
    public void SetFaceUp()
    {
        if (_frontSprite != null && _cardImage != null)
        {
            _cardImage.sprite = _frontSprite;
        }
    }
}