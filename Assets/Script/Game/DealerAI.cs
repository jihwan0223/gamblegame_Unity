using System.Collections;
using UnityEngine;

public class DealerAI : MonoBehaviour
{
    public GroqAI groqAI;
    public PlayerHand dealerHand;
    public DeckManager deckManager;
    public GameController gameController;

    public IEnumerator PlayTurn(int pScore)
    {
        // 1. 모든 참조가 유효한지 먼저 검사
        if (dealerHand == null) { Debug.LogError("DealerHand가 연결되지 않았습니다!"); yield break; }
        if (groqAI == null) { Debug.LogError("groqAI가 연결되지 않았습니다!"); yield break; }
        if (deckManager == null) { Debug.LogError("DeckManager가 연결되지 않았습니다!"); yield break; }

        Debug.Log("<color=cyan>딜러 턴 시작 (Gemini 모드)</color>");
        bool isDealerTurn = true;

        while (isDealerTurn)
        {
            // 27번 줄 에러 방지: 점수 계산 전 데이터 확인
            int dScore = dealerHand.GetTotalScore();
            Debug.Log($"현재 딜러 점수: {dScore}");

            if (dScore >= 21) break;

            string decision = "";
            bool isWaiting = true;

            //  결정 요청
            groqAI.GetDecision(dScore, pScore, (result) => {
                decision = result;
                isWaiting = false;
            });

            // 응답 대기 (무한 대기 방지)
            float timer = 0;
            while (isWaiting && timer < 5f) // 5초 넘으면 자동 Stay
            {
                timer += Time.deltaTime;
                yield return null;
            }

            if (timer >= 5f) decision = "Stay";

            Debug.Log($"Gemini Decision: {decision}");

            if (decision == "Hit")
            {
                Card card = deckManager.DrawCard();
                if (card != null)
                {
                    dealerHand.AddCard(card);
                    yield return new WaitForSeconds(1.5f);
                }
                else break;
            }
            else
            {
                isDealerTurn = false;
            }
        }

        yield return new WaitForSeconds(1.0f);
        gameController.DetermineWinner();
    }
}