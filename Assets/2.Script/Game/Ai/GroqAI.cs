using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class GroqAI : MonoBehaviour
{
    private string apiKey;
    private const string url = "https://api.groq.com/openai/v1/chat/completions";

    public delegate void OnResponse(string message);

    private void Awake()
    {
        string path = System.IO.Path.Combine(Application.streamingAssetsPath, "config.json");
        string json = System.IO.File.ReadAllText(path);
        apiKey = JsonUtility.FromJson<Config>(json).apiKey;
    }

    [System.Serializable]
    private class Config { public string apiKey; }

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
                $"You are a sophisticated blackjack dealer at a high-end casino. " +
                $"Player: {pScore}, Dealer: {dScore}, Result: {result}. " +
                "Say ONE elegant line naturally mentioning the scores and result. " +
                "Rules: Polite Korean tone. Max 40 characters. Composed and refined. Dialogue only. Correct Korean spelling. " +
                "Example(player_win): '1점 차이군요. 아슬아슬한 승리였습니다.' " +
                "Example(dealer_win): '딜러가 앞섰습니다. 다음 기회를 노려보시죠.' " +
                "Example(player_bust): '21을 넘기셨군요. 한 장이 과했습니다.' " +
                "Example(draw): '같은 점수로 무승부입니다. 드문 일이죠.' " +
                "IMPORTANT: Response must be in Korean only.";
        }
        else
        {
            systemRole =
                $"You are a sophisticated blackjack dealer at a high-end casino. " +
                $"Player: {pScore}, Dealer: {dScore}, Result: {result}. " +
                "Say ONE elegant line naturally mentioning the scores and result. " +
                "Rules: Polite tone. Max 15 words. Composed and refined. Dialogue only. " +
                "Example(player_win): 'Just one point ahead — a close victory, well played.' " +
                "Example(dealer_win): 'The dealer edges ahead. Better luck next time.' " +
                "Example(player_bust): 'Over 21 — one card too many, I'm afraid.' " +
                "Example(draw): 'Equal scores — a rare tie. Fate was undecided today.'";
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