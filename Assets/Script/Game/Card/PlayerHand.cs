using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerHand : MonoBehaviour
{
    [Header("생성할 카드 프리팹")]
    public GameObject cradPrefab;

    [Header("배치 / 애니메이션 설정")]
    public float closedSpacing = 80f;
    public float openSpacing = 80f;
    public float lerpSpeed = 10f;

    [Header("UI 설정")]
    // 2. 점수를 표시할 텍스트 컴포넌트 (인스펙터에서 드래그 앤 드롭)
    public TextMeshProUGUI scoreText; 
    public string scorePrefix = "Score: "; // 점수 앞에 붙을 텍스트 (예: "Player: ")

    private List<Card> _cardData = new List<Card>();
    private List<Transform> _cardTransforms = new List<Transform>();

    private void Start()
    {
        // 시작 시 점수 초기화
        UpdateScoreUI(0);
    }

    private void Update()
    {
        bool isExpanding = false;
        if (UnityEngine.InputSystem.Mouse.current != null)
        {
            isExpanding = UnityEngine.InputSystem.Mouse.current.leftButton.isPressed;
        }

        if (_cardTransforms.Count > 0)
        {
            float targetSpacing = isExpanding ? openSpacing : closedSpacing;
            UpdateLayout(targetSpacing);
        }
    }

    public void AddCard(Card newCard)
    {
        _cardData.Add(newCard);
        GameObject cardObj = Instantiate(cradPrefab, transform);
        _cardTransforms.Add(cardObj.transform);

        if (cardObj.GetComponent<CardView>() != null)
        {
            cardObj.GetComponent<CardView>().Setup(newCard);
        }

        // 3. 점수 계산 및 UI 업데이트
        int currentScore = GetTotalScore();
        UpdateScoreUI(currentScore);

        // --- [추가된 부분] 콘솔창에 플레이어 점수 출력 ---
        Debug.Log($"<color=yellow>[Player]</color> 현재 점수: {currentScore}");
        // ----------------------------------------------

        if (currentScore > 21)
        {
            Debug.Log("<color=red>Bust!</color> 21점을 넘었습니다."); // 여기도 조금 더 자세히 수정
            if (scoreText != null) scoreText.color = Color.red; 
        }
    }

    // UI 텍스트를 갱신하는 전용 함수
    private void UpdateScoreUI(int score)
    {
        if (scoreText != null)
        {
            scoreText.text = $"{scorePrefix}{score}";
        }
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
                    _cardTransforms[i].localPosition, 
                    targetPos, 
                    Time.deltaTime * lerpSpeed
                );
            }
        }
    }

    public int GetTotalScore()
    {
        return BlackjackScore.CalculateScore(_cardData);
    }

    public void ClearHand()
{
    // 1. 데이터 리스트 비우기
    _cardData.Clear();
    _cardTransforms.Clear();

    // 2. 화면에 생성된 모든 카드 오브젝트(자식들) 삭제
    // transform에 붙어있는 모든 자식을 루프 돌며 파괴합니다.
    foreach (Transform child in transform)
    {
        Destroy(child.gameObject);
    }

    // 3. 점수 및 UI 초기화
    UpdateScoreUI(0);
    if (scoreText != null) 
    {
        scoreText.color = Color.white;
    }

    Debug.Log($"{gameObject.name} 핸드 클리어.");
    }
}