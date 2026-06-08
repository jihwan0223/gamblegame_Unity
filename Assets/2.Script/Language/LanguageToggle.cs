using Lean.Localization;
using UnityEngine;

public class LanguageToggle : MonoBehaviour
{
    public static LanguageToggle Instance;

    public bool _isKorean = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (PlayerPrefs.HasKey("Language"))
        {
            _isKorean = PlayerPrefs.GetInt("Language") == 1;
        }
        else
        {
            _isKorean = false; // 기본값 영어
        }

        LeanLocalization.SetCurrentLanguageAll(_isKorean ? "Korean" : "English");
    }

    public void KoreanToggle()
    {
        LeanLocalization.SetCurrentLanguageAll("Korean");
        _isKorean = true;
        PlayerPrefs.SetInt("Language", 1);
        Debug.Log("언어: 한국어");
    }

    public void EnglishToggle()
    {
        LeanLocalization.SetCurrentLanguageAll("English");
        _isKorean = false;
        PlayerPrefs.SetInt("Language", 0);
        Debug.Log("언어: 영어");
    }
}