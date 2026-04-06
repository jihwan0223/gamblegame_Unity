using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class GroqAI : MonoBehaviour
{
    [SerializeField] private string apiKey = "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";

    public delegate void DecisionCallback(string result);

    public void GetDecision(int dealer, int player, DecisionCallback callback)
    {
        StartCoroutine(PostRequest(dealer, player, callback));
    }

    private IEnumerator PostRequest(int dealer, int player, DecisionCallback callback)
    {
        string url = "https://api.groq.com/openai/v1/chat/completions";

        // 프롬프트 최소화 + 출력 강제
        string prompt = $"Dealer:{dealer} Player:{player}. Answer ONLY one word: Hit or Stay.";

        string jsonData = @"
        {
            ""model"": ""llama-3.1-8b-instant"",
            ""messages"": [
                {""role"": ""system"", ""content"": ""You are a blackjack dealer AI. Only reply Hit or Stay.""}, 
                {""role"": ""user"", ""content"": """ + prompt + @"""}
            ],
            ""temperature"": 0,
            ""max_tokens"": 5
        }";

        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);

        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();

            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", "Bearer " + apiKey);

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string response = request.downloadHandler.text;
                Debug.Log("[Groq 응답] " + response);

                // 안정적인 판별
                string upper = response.ToUpper();

                if (upper.Contains("HIT"))
                    callback("Hit");
                else if (upper.Contains("STAY"))
                    callback("Stay");
                else
                    callback("Stay"); // 예외 방어
            }
            else
            {
                Debug.LogError("[에러] " + request.error + "\n" + request.downloadHandler.text);
                callback("Stay");
            }
        }
    }
}