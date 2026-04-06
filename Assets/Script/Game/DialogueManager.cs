using System.Collections.Generic;

public static class DialogueManager
{
    public enum Language { KR, EN }
    public static Language currentLang = Language.EN; // 기본 언어 설정

    // {0}: 플레이어 점수, {1}: 딜러 점수
    private static Dictionary<string, string> krDeals = new Dictionary<string, string>()
    {
        { "Hit_Easy", "음, {1}점이라... 한 장 더 뽑아볼게요!" },
        { "Stay_Normal", "{1}점이면 충분해요. 당신의 {0}점을 이길 수 있을까요?" },
        { "Hard_Provoke", "겨우 {0}점인가요? 제 {1}점으로 박살 내드리죠." },
        { "Greet_Easy", "반가워요! 1라운드는 살살 할게요." },
        { "Greet_Normal", "2라운드군요. 이제 정석대로 갑니다." },
        { "Greet_Hard", "마지막 라운드... 진심으로 상대해주죠." }
    };

    private static Dictionary<string, string> enDeals = new Dictionary<string, string>()
    {
        { "Hit_Easy", "Well, {1} score... Let's draw one more!" },
        { "Stay_Normal", "{1} is enough. Can I beat your {0}?" },
        { "Hard_Provoke", "Only {0}? My {1} will crush you." },
        { "Greet_Easy", "Hi! I'll go easy on you for Round 1." },
        { "Greet_Normal", "Round 2. Let's play by the book." },
        { "Greet_Hard", "Final Round... I'm playing for real now." }
    };

    public static string GetSmartText(string key, int pScore, int dScore)
    {
        var dict = (currentLang == Language.KR) ? krDeals : enDeals;
        if (dict.ContainsKey(key))
            return string.Format(dict[key], pScore, dScore);
        
        return key; // 키를 못 찾으면 키값 그대로 출력 (에러 방지)
    }
}