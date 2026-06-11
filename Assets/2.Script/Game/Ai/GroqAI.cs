using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class GroqAI : MonoBehaviour
{
    private string apiKey = "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";
    private const string url = "https://api.groq.com/openai/v1/chat/completions";

    public delegate void OnResponse(string message);

    public void GetDealerResponse(int pScore, int dScore, OnResponse callback)
    {
        StartCoroutine(PostRequest(pScore, dScore, callback));
    }

    private IEnumerator PostRequest(int pScore, int dScore, OnResponse callback)
    {
        // 승패 코드에서 직접 판단
        string result;
        if (pScore > 21)       result = "player_bust";
        else if (dScore > 21)  result = "dealer_bust";
        else if (pScore > dScore) result = "player_win";
        else if (dScore > pScore) result = "dealer_win";
        else                   result = "draw";
        Debug.Log($"pScore: {pScore}, dScore: {dScore}, result: {result}");

        string systemRole;

        if (LanguageToggle.Instance._isKorean)
        {
            systemRole =
                $"당신은 냉소적이고 카리스마 넘치는 블랙잭 딜러입니다. 결과: {result}. " +
                "이 결과에 어울리는 짧은 대사를 말하세요. " +
                "조건: 반말. 20자 이내. 비꼬거나 여유있는 말투. 숫자/설명 금지. 대사만. 맞춤법 정확히. " +
                "예시(player_win): '운이 좋았군, 다음엔 없어.' " +
                "예시(dealer_win): '처음부터 결과는 정해져 있었어.' " +
                "예시(player_bust): '욕심이 화를 불렀지.' " +
                "예시(draw): '오늘은 봐주는 거야.'";
        }
        else
        {
            systemRole =
                $"You are a cynical and charismatic blackjack dealer. Result: {result}. " +
                "Say a short line matching this result. " +
                "Rules: Max 8 words. Sarcastic or cool tone. No numbers, no explanations. Dialogue only. " +
                "Example(player_win): 'Luck won't save you next time.' " +
                "Example(dealer_win): 'The house always wins.' " +
                "Example(player_bust): 'Greed got the better of you.' " +
                "Example(draw): 'Consider yourself lucky today.'";
        }

        RequestData data = new RequestData();
        data.model = "llama-3.3-70b-versatile";
        data.messages = new Message[]
        {
            new Message { role = "system", content = systemRole },
            new Message
            {
                role = "user",
                content = LanguageToggle.Instance._isKorean
                    ? "게임이 끝났습니다." : "The game is over."
            }
        };

        string jsonData = JsonUtility.ToJson(data);
        byte[] bodyRaw  = Encoding.UTF8.GetBytes(jsonData);

        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            request.uploadHandler   = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", "Bearer " + apiKey);

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                ResponseData res = JsonUtility.FromJson<ResponseData>(request.downloadHandler.text);
                callback(res.choices[0].message.content);
            }
            else
            {
                Debug.LogError($"AI 에러: {request.error}");
                callback(LanguageToggle.Instance._isKorean
                    ? "...게임에 에러가 발생했습니다."
                    : "...Game Error.");
            }
        }
    }

    [System.Serializable] public class RequestData  { public string model; public Message[] messages; }
    [System.Serializable] public class Message      { public string role; public string content; }
    [System.Serializable] public class ResponseData { public Choice[] choices; }
    [System.Serializable] public class Choice       { public Message message; }
}