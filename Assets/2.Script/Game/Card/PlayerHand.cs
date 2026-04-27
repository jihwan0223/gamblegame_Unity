using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SocialPlatforms.Impl;

public class PlayerHand : MonoBehaviour
{
    [Header("딜러면 체크")]
    public bool isDealer = false;    // 딜러인지 체크
    public bool isDealerTurnStarted = false; 

    [Header("생성할 카드 프리팹")]
    public GameObject cradPrefab;

    [Header("배치 / 애니메이션 설정")]
    public float closedSpacing = 80f;
    public float openSpacing = 80f;
    public float lerpSpeed = 10f;

    [Header("UI 설정")]
    public TextMeshProUGUI scoreText; 
    public string scorePrefix = "Score: ";

    private List<Card> _cardData = new List<Card>();
    private List<Transform> _cardTransforms = new List<Transform>();

    private void Start() => UpdateScoreUI(0);

    private void Update()
    {
        bool isExpanding = false;
        if (UnityEngine.InputSystem.Mouse.current != null)
            isExpanding = UnityEngine.InputSystem.Mouse.current.leftButton.isPressed;

        if (_cardTransforms.Count > 0)
        {
            float targetSpacing = isExpanding ? openSpacing : closedSpacing;
            UpdateLayout(targetSpacing);
        }
    }

    // 카드 추가
    public void AddCard(Card newCard)
    {
        _cardData.Add(newCard);
        GameObject cardObj = Instantiate(cradPrefab, transform);
        _cardTransforms.Add(cardObj.transform);

        CardView cardView = cardObj.GetComponent<CardView>();
        if (cardView != null)
        {
            cardView.Setup(newCard);

            if (isDealer && _cardTransforms.Count > 1 && !isDealerTurnStarted)
            {
                cardView.SetFaceDown();
            }
        }

        // UI 갱신
        RefreshScoreDisplay();
    }

    // 모든 카드를 앞면으로 공개
    public void RevealAllCards()
    {
        isDealerTurnStarted = true;
        foreach (Transform t in _cardTransforms)
        {
            CardView cv = t.GetComponent<CardView>();
            if (cv != null)
            {
                cv.SetFaceUp();
            }
        }
        RefreshScoreDisplay();
    }

    private void RefreshScoreDisplay()
    {   // 점수 계산
        int currentScore = GetTotalScore();

        if (isDealer && !isDealerTurnStarted)
        {
            int firstCardScore = BlackjackScore.CalculateScore(new List<Card> { _cardData[0]});

            if (scoreText != null)
            {
                scoreText.text = $"{scorePrefix}{firstCardScore}";
                scoreText.color = Color.white;
            }
        }
        else
        {
            UpdateScoreUI(currentScore);

            if(currentScore > 21)
            {
                if(scoreText != null) scoreText.color = Color.red;
            }
        }

        string owner = isDealer ? "Dealer" : "Player";
        Debug.Log($"<color=yellow>[{owner}]</color 점수: {currentScore}");
    }

    private void UpdateScoreUIHidden()
    {
        if (scoreText != null)
        {
            scoreText.text = $"{scorePrefix}?";
            scoreText.color = Color.white;
        }
    }

    private void UpdateScoreUI(int score)
    {
        if (scoreText != null) scoreText.text = $"{scorePrefix}{score}";
    }

    private void UpdateLayout(float spacing)
    {
        int count = _cardTransforms.Count;
        float centerOffset = (count - 1) / 2f;

        for (int i = 0; i < count; i++)
        {
            Vector3 targetPos = new Vector3((i - centerOffset) * spacing, 0, i * -0.05f);
            if (Vector3.SqrMagnitude(_cardTransforms[i].localPosition - targetPos) > 0.0001f)
            {
                _cardTransforms[i].localPosition = Vector3.Lerp(
                    _cardTransforms[i].localPosition, targetPos, Time.deltaTime * lerpSpeed);
            }
        }
    }

    public int GetTotalScore() => BlackjackScore.CalculateScore(_cardData);

    public void ClearHand()
    {
        isDealerTurnStarted = false;
        _cardData.Clear();
        _cardTransforms.Clear();

        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        UpdateScoreUI(0);

        if(scoreText != null)
        {
            scoreText.color = Color.white;
        }

        Debug.Log($"{gameObject.name} 핸드 클리어.");
    }
}