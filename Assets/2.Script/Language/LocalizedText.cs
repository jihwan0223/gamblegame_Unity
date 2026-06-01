using TMPro;
using UnityEngine;

public class LocalizedText : MonoBehaviour
{
    public string key;

    private TMP_Text text;

    void Awake()
    {
        text = GetComponent<TMP_Text>();

        // key가 비어있으면 오브젝트 이름 사용
        if (string.IsNullOrEmpty(key))
        {
            key = gameObject.name;
        }
    }

    void Start()
    {
        UpdateText();
    }

    public void UpdateText()
    {
        if (LanguageManager.Instance == null) return;
        if (text == null) return;

        text.text = LanguageManager.Instance.GetText(key);
    }
}
