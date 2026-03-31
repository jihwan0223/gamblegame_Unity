using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;
public class PlayerHand : MonoBehaviour
{
    [Header("생성할 카드 프리팹")]

    public GameObject cradPrefab;

    [Header("배치 / 애니메이션 설정")]
    public float closedSpacing = 0.3f;      // 카드 간격
    public float openSpacing = 1.2f;        // 펼쳐질때 간격
    public float lerpSpeed = 10f;           // 카드 이동 속도


    private List<Card> _cardData = new List<Card>();
    private List<Transform> _cardTransforms = new List<Transform>();
    private bool _lastExpandingState;


    private void Update()
{
    bool isExpanding = false;

    // 마우스 왼쪽 버튼을 누르고 있는지 체크
    if (UnityEngine.InputSystem.Mouse.current != null)
    {
        isExpanding = UnityEngine.InputSystem.Mouse.current.leftButton.isPressed;
    }

    // 카드 개수가 있을 때만 레이아웃 업데이트
    if (_cardTransforms.Count > 0)
    {
        // 누르고 있으면 OpenSpacing, 떼면 ClosedSpacing으로 부드럽게 이동
        float targetSpacing = isExpanding ? openSpacing : closedSpacing;
        UpdateLayout(targetSpacing);
    }
}

    // 카드 추가 / 점수 계산
    public void AddCard(Card newCard)
    {
        _cardData.Add(newCard);

        GameObject cardObj = Instantiate(cradPrefab, transform);

        _cardTransforms.Add(cardObj.transform);

        if (cardObj.GetComponent<CardView>() != null)
        {
            cardObj.GetComponent<CardView>().Setup(newCard);
        }


        // 점수 계산
        int currentScore = BlackjackScore.CalculateScore(_cardData);
        Debug.Log($"<color=yellow>[Player]</color> 현재 점수: {currentScore}");

        if (currentScore > 21)
        {
            Debug.Log("<color=red>Bust!</color> 21점을 넘었습니다.");
        }
    }

    private void UpdateLayout(float spacing)
    {
        int count = _cardTransforms.Count;
        float centerOffset = (count - 1) / 2f; // 중앙 정렬을 위한 오프셋 값 계산

        for (int i = 0; i < count; i++)
        {
            // i번째 카드가 가야 할 목표 로컬 좌표 계산
            // Z축은 카드 겹침 현상(Z-Fighting) 방지를 위해 순서대로 살짝 띄움
            Vector3 targetPos = new Vector3((i - centerOffset) * spacing, 0, i * -0.05f);
            
            // 최적화: 현재 위치와 목표 위치의 거리 차이가 아주 작을 때는 계산 생략
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
}