using System;
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
        // 1. 프롬프트 설정 (영어 사용, 승패 판단 포함)
        string systemRole;

        if (LanguageToggle.Instance._isKorean)
        {
            systemRole =
                $"당신은 블랙잭 딜러입니다. 플레이어:{pScore}, 딜러:{dScore}. " +
                "누가 이겼는지 판단하고 짧고 카리스마 있는 한국어 대사를 2문장 이하로 말하세요.";
        }
        else
        {
            systemRole =
                $"You are a blackjack dealer. Player:{pScore}, Dealer:{dScore}. " +
                "Decide who won and say a short charismatic line in English (Max 2 sentences).";
        }

        // 2. JSON 데이터 구성
        RequestData data = new RequestData();
        data.model = "llama-3.1-8b-instant";
        data.messages = new Message[] { 
            new Message { role = "system", content = systemRole },
            new Message
            {
                role = "user",
                content = LanguageToggle.Instance._isKorean
                ? "게임이 끝났습니다." : "The game is over."
            }
        };

        string jsonData = JsonUtility.ToJson(data);
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);

        // 3. 통신 실행
        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
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
                callback(
                LanguageToggle.Instance._isKorean
                    ? "...게임에 에러가 발생했습니다."
                    : "...Game Error."
                    );
            }
        }
    }

    // JSON 파싱용 구조체
    [System.Serializable] public class RequestData { public string model; public Message[] messages; }
    [System.Serializable] public class Message { public string role; public string content; }
    [System.Serializable] public class ResponseData { public Choice[] choices; }
    [System.Serializable] public class Choice { public Message message; }
}