using System.ComponentModel;

[System.Serializable]

public class Card
{
    public enum CardSiot { Heart, Diamond, Club, Spade }

    public CardSiot suit;
    public int rank;        // 카드 점수

    public Card(CardSiot suit, int rank)
    {
        this.suit = suit;
        this.rank = rank;
    }

    // 점수 계산
    public int GetRawValue()
    {
        if (rank >= 11)
        {
            return 10;
        }

        // A 카드 나올시 11로 계산후 BlackjackScore.cs에서 결정
        if (rank == 1)
        {
            return 11;
        }
        return rank;
    }
}