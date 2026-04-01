using System.Collections;
using UnityEngine;
using TMPro;

public class DealerAI : MonoBehaviour
{
    public TextMeshProUGUI speechText;
    public enum Difficulty { Easy, Normal, Hard }
    public Difficulty currentDifficulty;

    // GameManager에서 호출하여 난이도와 인삿말 설정
    public void SetDifficulty(int round)
    {
        if (round == 1) currentDifficulty = Difficulty.Easy;
        else if (round == 2) currentDifficulty = Difficulty.Normal;
        else currentDifficulty = Difficulty.Hard;

        Speak("Greet_" + currentDifficulty.ToString(), 0, 0);
    }

    public IEnumerator PlayTurn(int pScore, PlayerHand dealerHand, DeckManager deck)
    {
        bool isDone = false;

        while (!isDone)
        {
            int dScore = dealerHand.GetTotalScore();
            yield return new WaitForSeconds(1.5f); // 생각하는 시간 연출

            // 난이도별 로직 결정
            if (currentDifficulty == Difficulty.Easy)
            {
                if (dScore < 15) yield return DealerHit(dealerHand, deck, "Hit_Easy", pScore);
                else { isDone = true; Speak("Stay_Normal", pScore, dScore); }
            }
            else if (currentDifficulty == Difficulty.Normal)
            {
                if (dScore < 17) yield return DealerHit(dealerHand, deck, "Hit_Easy", pScore);
                else { isDone = true; Speak("Stay_Normal", pScore, dScore); }
            }
            else // Hard 모드
            {
                if (dScore <= pScore && dScore < 20) yield return DealerHit(dealerHand, deck, "Hard_Provoke", pScore);
                else { isDone = true; Speak("Stay_Normal", pScore, dScore); }
            }
        }
    }

    private IEnumerator DealerHit(PlayerHand hand, DeckManager deck, string key, int pScore)
    {
        Speak(key, pScore, hand.GetTotalScore());
        yield return new WaitForSeconds(1f);
        Card card = deck.DrawCard();
        if (card != null) hand.AddCard(card);
    }

    public void Speak(string key, int pScore, int dScore)
    {
        if (speechText != null)
            speechText.text = DialogueManager.GetSmartText(key, pScore, dScore);
    }
}