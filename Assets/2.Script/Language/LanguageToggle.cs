using Lean.Localization;
using NUnit.Framework;
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
            _isKorean = Application.systemLanguage == SystemLanguage.Korean;
        }

        LeanLocalization.SetCurrentLanguageAll(
            _isKorean ? "Korean" : "English"
        );
    }

    public void KoreanToggle()
    {
        LeanLocalization.SetCurrentLanguageAll("Korean");
        _isKorean = true;
        Debug.Log("언어: 한국어");
    }
    public void EnglishToggle()
    {
        LeanLocalization.SetCurrentLanguageAll("English");
        _isKorean = false;
        Debug.Log("언어 : 영어");
    }
}