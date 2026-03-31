using System.Linq;
using System.Collections.Generic;
using System.Threading;

public class BlackjackScore
{
    public static int CalculateScore(List<Card> hand)
    {
        int total = hand.Sum(card => card.GetRawValue());       // 점수 게산
        int aceCount = hand.Count(card => card.rank == 1);      // A 개수

        // 21 넘으면 A카드 계산
        while (total > 21 && aceCount > 0)
        {
            total -= 10;
            aceCount--;
        }

        return total;
    }
}
